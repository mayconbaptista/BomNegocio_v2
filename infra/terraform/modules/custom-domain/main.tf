# Dominio proprio (registrado/gerenciado no Cloudflare) servindo o CloudFront.
#
#   Cloudflare = somente DNS (nuvem cinza, proxied = false)
#   CloudFront = CDN + TLS, com certificado ACM gratuito
#
# Por que nao "proxied" (nuvem laranja)? Seriam duas CDNs encadeadas: dois
# caches com regras diferentes, dois pontos de TLS e erros dificeis de
# diagnosticar, sem ganho de custo (o CloudFront ja esta no always-free).

terraform {
  required_providers {
    aws = {
      source = "hashicorp/aws"
      # CloudFront so aceita certificados ACM emitidos em us-east-1.
      configuration_aliases = [aws.us_east_1]
    }
    cloudflare = {
      source = "cloudflare/cloudflare"
    }
  }
}

locals {
  fqdn = var.subdomain == "" ? var.domain_name : "${var.subdomain}.${var.domain_name}"
}

resource "aws_acm_certificate" "app" {
  provider          = aws.us_east_1
  domain_name       = local.fqdn
  validation_method = "DNS"

  # Na troca de dominio o certificado novo nasce antes de o antigo sair da
  # distribuicao (senao o CloudFront ficaria sem certificado valido).
  lifecycle {
    create_before_destroy = true
  }
}

# Registro CNAME que prova ao ACM que voce controla o dominio. Ele precisa
# PERMANECER no DNS: a renovacao automatica (a cada ~13 meses) usa o mesmo
# registro.
resource "cloudflare_dns_record" "acm_validation" {
  for_each = {
    for option in aws_acm_certificate.app.domain_validation_options : option.domain_name => {
      name    = trimsuffix(option.resource_record_name, ".")
      type    = option.resource_record_type
      content = trimsuffix(option.resource_record_value, ".")
    }
  }

  zone_id = var.cloudflare_zone_id
  name    = each.value.name
  type    = each.value.type
  content = each.value.content
  ttl     = 1 # 1 = automatico
  proxied = false
  comment = "Validacao ACM ${local.fqdn} (Terraform)"
}

resource "aws_acm_certificate_validation" "app" {
  provider        = aws.us_east_1
  certificate_arn = aws_acm_certificate.app.arn

  depends_on = [cloudflare_dns_record.acm_validation]
}

# No dominio raiz o Cloudflare aplica "CNAME flattening" automaticamente, entao
# CNAME funciona tanto em subdominio quanto no apex.
resource "cloudflare_dns_record" "app" {
  zone_id = var.cloudflare_zone_id
  name    = local.fqdn
  type    = "CNAME"
  content = var.cloudfront_domain_name
  ttl     = 1
  proxied = false
  comment = "BnEshop ${var.environment} -> CloudFront (Terraform)"
}
