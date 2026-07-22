variable "project_name" {
  description = "Nome do projeto, usado como prefixo/tag em todos os recursos"
  type        = string
}

variable "environment" {
  description = "Nome do ambiente (ex: staging, production)"
  type        = string
}

variable "vpc_cidr" {
  description = "Bloco CIDR da VPC"
  type        = string
  default     = "10.0.0.0/16"
}

variable "public_subnet_cidr" {
  description = "Bloco CIDR da subnet publica onde a EC2 vai viver"
  type        = string
  default     = "10.0.1.0/24"
}

variable "availability_zone" {
  description = "AZ da subnet publica. Uma unica AZ e suficiente pois so existe 1 instancia EC2"
  type        = string
}
