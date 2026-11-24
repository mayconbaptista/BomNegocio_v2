locals {
  name          = "${var.project_name}-${var.environment}"
  spa_origin    = "spa-s3"
  media_origin  = "catalog-s3"
  api_origin    = "api-ec2"
  origin_header = "X-Origin-Verify"
}

# ---------------------------------------------------------------------------
# Bucket do Angular (privado; somente o CloudFront le via OAC)
# ---------------------------------------------------------------------------
resource "aws_s3_bucket" "spa" {
  bucket        = "${local.name}-${var.aws_account_id}-spa"
  force_destroy = true # conteudo e reproduzivel a partir do build
}

resource "aws_s3_bucket_public_access_block" "spa" {
  bucket                  = aws_s3_bucket.spa.id
  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}

resource "aws_s3_bucket_server_side_encryption_configuration" "spa" {
  bucket = aws_s3_bucket.spa.id
  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}

# OAC (sucessor do OAI): o CloudFront assina as requisicoes com SigV4 e o
# bucket policy aceita somente esta distribuicao (AWS:SourceArn).
resource "aws_cloudfront_origin_access_control" "s3" {
  name                              = "${local.name}-s3-oac"
  origin_access_control_origin_type = "s3"
  signing_behavior                  = "always"
  signing_protocol                  = "sigv4"
}

# ---------------------------------------------------------------------------
# Politicas de cache
# ---------------------------------------------------------------------------
data "aws_cloudfront_cache_policy" "caching_optimized" {
  name = "Managed-CachingOptimized"
}

data "aws_cloudfront_origin_request_policy" "all_viewer_except_host" {
  name = "Managed-AllViewerExceptHostHeader"
}

data "aws_cloudfront_response_headers_policy" "security_headers" {
  name = "Managed-SecurityHeadersPolicy"
}

# A API nao deve ser cacheada, mas o header Authorization so e repassado em
# GET/HEAD se fizer parte da cache key, e a policy gerenciada CachingDisabled
# nao aceita headers. Solucao: TTL default 0 / max 1s com Authorization na key.
# Efeito pratico: sem cache, a menos que a API envie Cache-Control explicito.
resource "aws_cloudfront_cache_policy" "api" {
  name        = "${local.name}-api-no-cache"
  min_ttl     = 0
  default_ttl = 0
  max_ttl     = 1

  parameters_in_cache_key_and_forwarded_to_origin {
    enable_accept_encoding_gzip   = true
    enable_accept_encoding_brotli = true

    headers_config {
      header_behavior = "whitelist"
      headers {
        items = ["Authorization"]
      }
    }
    cookies_config {
      cookie_behavior = "none"
    }
    query_strings_config {
      query_string_behavior = "all"
    }
  }
}

resource "aws_cloudfront_function" "spa_rewrite" {
  name    = "${local.name}-spa-rewrite"
  runtime = "cloudfront-js-2.0"
  publish = true
  code    = file("${path.module}/functions/spa-rewrite.js")
}

resource "aws_cloudfront_function" "media_strip_prefix" {
  name    = "${local.name}-media-strip"
  runtime = "cloudfront-js-2.0"
  publish = true
  code    = file("${path.module}/functions/media-strip-prefix.js")
}

# ---------------------------------------------------------------------------
# Distribuicao unica: /  -> Angular | /api/* -> EC2 | /media/* -> catalogo
# Mesma origem para o browser = sem CORS.
# ---------------------------------------------------------------------------
resource "aws_cloudfront_distribution" "main" {
  enabled             = true
  is_ipv6_enabled     = true
  comment             = "${local.name} SPA + API + media"
  default_root_object = "index.html"
  price_class         = "PriceClass_100"
  http_version        = "http2and3"

  origin {
    origin_id                = local.spa_origin
    domain_name              = aws_s3_bucket.spa.bucket_regional_domain_name
    origin_access_control_id = aws_cloudfront_origin_access_control.s3.id
  }

  origin {
    origin_id                = local.media_origin
    domain_name              = var.catalog_bucket_regional_domain_name
    origin_access_control_id = aws_cloudfront_origin_access_control.s3.id
  }

  # Origem da API = hostname do Cloudflare Tunnel (ADR-012). O trecho
  # CloudFront -> Cloudflare e HTTPS (certificado Universal SSL valido para o
  # hostname) e Cloudflare -> EC2 vai pelo tunnel criptografado. O header
  # secreto continua: quem chamar o hostname de origem direto, sem passar pelo
  # CloudFront, recebe 404 do Traefik.
  origin {
    origin_id   = local.api_origin
    domain_name = var.api_origin_domain

    # Com a EC2 desligada a Cloudflare responde 530 na hora (tunnel offline).
    # Os timeouts curtos cobrem falhas de conexao com a propria Cloudflare.
    connection_attempts = 2
    connection_timeout  = 3

    custom_origin_config {
      http_port                = 80
      https_port               = 443
      origin_protocol_policy   = "https-only"
      origin_ssl_protocols     = ["TLSv1.2"]
      origin_read_timeout      = 30
      origin_keepalive_timeout = 5
    }

    custom_header {
      name  = local.origin_header
      value = var.origin_verify_secret
    }
  }

  ordered_cache_behavior {
    path_pattern             = "/api/*"
    target_origin_id         = local.api_origin
    viewer_protocol_policy   = "https-only"
    allowed_methods          = ["GET", "HEAD", "OPTIONS", "PUT", "POST", "PATCH", "DELETE"]
    cached_methods           = ["GET", "HEAD"]
    compress                 = true
    cache_policy_id          = aws_cloudfront_cache_policy.api.id
    origin_request_policy_id = data.aws_cloudfront_origin_request_policy.all_viewer_except_host.id
  }

  ordered_cache_behavior {
    path_pattern           = "/media/*"
    target_origin_id       = local.media_origin
    viewer_protocol_policy = "redirect-to-https"
    allowed_methods        = ["GET", "HEAD"]
    cached_methods         = ["GET", "HEAD"]
    compress               = true
    cache_policy_id        = data.aws_cloudfront_cache_policy.caching_optimized.id

    function_association {
      event_type   = "viewer-request"
      function_arn = aws_cloudfront_function.media_strip_prefix.arn
    }
  }

  default_cache_behavior {
    target_origin_id           = local.spa_origin
    viewer_protocol_policy     = "redirect-to-https"
    allowed_methods            = ["GET", "HEAD"]
    cached_methods             = ["GET", "HEAD"]
    compress                   = true
    cache_policy_id            = data.aws_cloudfront_cache_policy.caching_optimized.id
    response_headers_policy_id = data.aws_cloudfront_response_headers_policy.security_headers.id

    function_association {
      event_type   = "viewer-request"
      function_arn = aws_cloudfront_function.spa_rewrite.arn
    }
  }

  aliases = var.aliases

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  # Com dominio proprio: certificado ACM (SNI, gratuito). Sem dominio: o
  # certificado *.cloudfront.net padrao. "sni-only" evita o IP dedicado
  # (US$ 600/mes) e TLSv1.2_2021 corta protocolos e cifras legados.
  viewer_certificate {
    cloudfront_default_certificate = var.acm_certificate_arn == null
    acm_certificate_arn            = var.acm_certificate_arn
    ssl_support_method             = var.acm_certificate_arn == null ? null : "sni-only"
    minimum_protocol_version       = var.acm_certificate_arn == null ? "TLSv1" : "TLSv1.2_2021"
  }

  tags = {
    Name = "${local.name}-cdn"
  }
}

# ---------------------------------------------------------------------------
# Bucket policies: somente esta distribuicao le os objetos
# ---------------------------------------------------------------------------
data "aws_iam_policy_document" "cloudfront_read" {
  for_each = {
    spa   = aws_s3_bucket.spa.arn
    media = var.catalog_bucket_arn
  }

  statement {
    sid       = "AllowCloudFrontOAC"
    actions   = ["s3:GetObject"]
    resources = ["${each.value}/*"]

    principals {
      type        = "Service"
      identifiers = ["cloudfront.amazonaws.com"]
    }

    condition {
      test     = "StringEquals"
      variable = "AWS:SourceArn"
      values   = [aws_cloudfront_distribution.main.arn]
    }
  }
}

resource "aws_s3_bucket_policy" "spa" {
  bucket     = aws_s3_bucket.spa.id
  policy     = data.aws_iam_policy_document.cloudfront_read["spa"].json
  depends_on = [aws_s3_bucket_public_access_block.spa]
}

# O bucket de catalogo pertence ao modulo storage, mas a policy depende da
# distribuicao. Um bucket aceita uma unica policy: ela vive somente aqui.
resource "aws_s3_bucket_policy" "catalog" {
  bucket = var.catalog_bucket_id
  policy = data.aws_iam_policy_document.cloudfront_read["media"].json
}
