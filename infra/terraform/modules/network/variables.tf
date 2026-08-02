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

variable "availability_zones" {
  description = "Duas AZs: a primeira hospeda EC2/RDS e ambas compoem o DB subnet group"
  type        = list(string)

  validation {
    condition     = length(var.availability_zones) >= 2
    error_message = "Informe pelo menos duas availability zones."
  }
}

variable "private_db_subnet_cidrs" {
  description = "CIDRs das subnets privadas usadas pelo DB subnet group"
  type        = list(string)

  validation {
    condition     = length(var.private_db_subnet_cidrs) >= 2
    error_message = "Informe pelo menos dois CIDRs privados para o RDS."
  }
}
