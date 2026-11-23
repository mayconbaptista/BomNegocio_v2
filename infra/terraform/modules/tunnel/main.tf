# Cloudflare Tunnel como origem da API (ADR-012).
#
#   CloudFront --HTTPS--> Cloudflare (bneshop-origin.<dominio>) ==tunnel==> cloudflared --> Traefik
#
# O cloudflared, rodando na EC2, abre conexoes de SAIDA para a Cloudflare. Por
# isso a EC2 nao tem nenhuma porta de entrada nem precisa de IP fixo: o IPv4
# publico efemero (cobrado so com a instancia ligada) basta para a saida.

terraform {
  required_providers {
    cloudflare = {
      source = "cloudflare/cloudflare"
    }
  }
}

# config_src = "cloudflare": as regras de roteamento (ingress) ficam na API da
# Cloudflare, versionadas aqui, e nao em um config.yml dentro da EC2. Sem
# tunnel_secret explicito, a Cloudflare gera o segredo.
resource "cloudflare_zero_trust_tunnel_cloudflared" "api" {
  account_id = var.cloudflare_account_id
  name       = "${var.project_name}-${var.environment}"
  config_src = "cloudflare"
}

resource "cloudflare_zero_trust_tunnel_cloudflared_config" "api" {
  account_id = var.cloudflare_account_id
  tunnel_id  = cloudflare_zero_trust_tunnel_cloudflared.api.id

  config = {
    ingress = [
      {
        hostname = var.origin_hostname
        service  = var.origin_service
      },
      {
        # Regra final obrigatoria: qualquer outro hostname recebe 404.
        service = "http_status:404"
      }
    ]
  }
}

# Precisa ser "proxied": o trafego entra pela borda da Cloudflare e segue pelo
# tunnel. O hostname e de PRIMEIRO nivel (bneshop-origin.dominio, e nao
# origin.bneshop.dominio) porque o certificado gratuito (Universal SSL) cobre
# so *.dominio. O CloudFront valida esse certificado ao conectar por HTTPS.
resource "cloudflare_dns_record" "origin" {
  zone_id = var.cloudflare_zone_id
  name    = var.origin_hostname
  type    = "CNAME"
  content = "${cloudflare_zero_trust_tunnel_cloudflared.api.id}.cfargotunnel.com"
  ttl     = 1
  proxied = true
  comment = "Origem da API ${var.environment} via Cloudflare Tunnel (Terraform)"
}

# Token que o cloudflared usa para rodar ESTE tunnel. Ele nao permite editar
# DNS nem outros tunnels (bem diferente do API token usado pelo Terraform).
data "cloudflare_zero_trust_tunnel_cloudflared_token" "api" {
  account_id = var.cloudflare_account_id
  tunnel_id  = cloudflare_zero_trust_tunnel_cloudflared.api.id
}
