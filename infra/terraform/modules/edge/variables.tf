variable "project_name" { type = string }
variable "environment" { type = string }
variable "aws_account_id" { type = string }

variable "api_origin_domain" {
  description = "Hostname do Cloudflare Tunnel que leva ate o Traefik (HTTPS)"
  type        = string
}

variable "origin_verify_secret" {
  description = "Valor do header X-Origin-Verify enviado pelo CloudFront a origem"
  type        = string
  sensitive   = true
}

variable "catalog_bucket_id" { type = string }
variable "catalog_bucket_arn" { type = string }
variable "catalog_bucket_regional_domain_name" { type = string }

variable "aliases" {
  description = "Dominios proprios atendidos pela distribuicao (CNAMEs)"
  type        = list(string)
  default     = []
}

variable "acm_certificate_arn" {
  description = "Certificado ACM em us-east-1 cobrindo os aliases; null = certificado padrao do CloudFront"
  type        = string
  default     = null
}
