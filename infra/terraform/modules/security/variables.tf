variable "project_name" {
  description = "Nome do projeto, usado como prefixo em nomes/tags de recursos"
  type        = string
}

variable "environment" {
  description = "Nome do ambiente (ex: staging, production)"
  type        = string
}

variable "vpc_id" {
  description = "VPC onde o Security Group sera criado"
  type        = string
}

variable "cloudflare_api_token" {
  description = "Token de API do Cloudflare (permissao minima: Zone:DNS:Edit no dominio usado), consumido pelo Traefik no desafio DNS-01 do Let's Encrypt"
  type        = string
  sensitive   = true
}
