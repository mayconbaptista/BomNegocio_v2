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

variable "aws_region" {
  description = "Regiao usada para construir ARNs de SQS/SNS"
  type        = string
}

variable "aws_account_id" {
  description = "ID da conta AWS"
  type        = string
}

variable "parameter_path_prefix" {
  description = "Prefixo dos parametros que a EC2 pode ler"
  type        = string
}

variable "catalog_bucket_arn" {
  description = "ARN do bucket de imagens do catalogo"
  type        = string
}

variable "private_bucket_arn" {
  description = "ARN do bucket privado de artefatos/certificados"
  type        = string
}
