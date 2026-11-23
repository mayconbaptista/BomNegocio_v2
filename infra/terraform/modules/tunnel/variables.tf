variable "project_name" { type = string }
variable "environment" { type = string }

variable "cloudflare_account_id" {
  description = "Account ID da Cloudflare (painel do dominio > Overview > API)"
  type        = string
}

variable "cloudflare_zone_id" { type = string }

variable "origin_hostname" {
  description = "Hostname de primeiro nivel que o CloudFront usa como origem da API"
  type        = string
}

variable "origin_service" {
  description = "Destino dentro da rede Docker, visto pelo container cloudflared"
  type        = string
  default     = "http://traefik:80"
}
