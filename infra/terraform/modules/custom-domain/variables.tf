variable "environment" { type = string }

variable "cloudflare_zone_id" {
  description = "Zone ID do dominio no Cloudflare (painel do dominio > Overview > API)"
  type        = string
}

variable "domain_name" {
  description = "Dominio raiz, ex.: meudominio.com.br"
  type        = string
}

variable "subdomain" {
  description = "Subdominio da aplicacao; string vazia usa o dominio raiz"
  type        = string
}

variable "cloudfront_domain_name" {
  description = "dxxxx.cloudfront.net da distribuicao"
  type        = string
}
