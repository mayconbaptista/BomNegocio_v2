variable "project_name" {
  description = "Nome do projeto, usado como prefixo em nomes/tags de recursos"
  type        = string
  default     = "bomnegocio"
}

variable "aws_region" {
  description = "Regiao AWS. us-east-1 tem o maior numero de servicos e geralmente o menor custo"
  type        = string
  default     = "us-east-1"
}

variable "availability_zone" {
  description = "AZ onde a subnet publica e a EC2 serao criadas"
  type        = string
  default     = "us-east-1a"
}

variable "vpc_cidr" {
  description = "CIDR da VPC de staging"
  type        = string
  default     = "10.0.0.0/16"
}

variable "public_subnet_cidr" {
  description = "CIDR da subnet publica de staging"
  type        = string
  default     = "10.0.1.0/24"
}

variable "cloudflare_api_token" {
  description = "Token de API do Cloudflare (permissao minima: Zone:DNS:Edit no dominio usado). Fornecer via terraform.tfvars (nao commitado) ou variavel de ambiente TF_VAR_cloudflare_api_token"
  type        = string
  sensitive   = true
}
