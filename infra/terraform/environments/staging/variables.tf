variable "project_name" {
  description = "Prefixo usado em nomes e tags"
  type        = string
  default     = "bnshop"
}

variable "aws_region" {
  type    = string
  default = "us-east-1"
}

variable "availability_zones" {
  type    = list(string)
  default = ["us-east-1a", "us-east-1b"]
}

variable "vpc_cidr" {
  type    = string
  default = "10.0.0.0/16"
}

variable "public_subnet_cidr" {
  type    = string
  default = "10.0.1.0/24"
}

variable "private_db_subnet_cidrs" {
  type    = list(string)
  default = ["10.0.11.0/24", "10.0.12.0/24"]
}

variable "instance_type" {
  description = "t4g.small oferece 2 GiB; confirme elegibilidade no console antes do apply"
  type        = string
  default     = "t4g.small"
}

variable "instance_architecture" {
  type    = string
  default = "arm64"
}

variable "root_volume_size" {
  type    = number
  default = 16
}

variable "docker_compose_version" {
  type    = string
  default = "2.40.3"
}

variable "db_instance_class" {
  type    = string
  default = "db.t4g.micro"
}

variable "db_allocated_storage" {
  type    = number
  default = 20
}

variable "db_master_username" {
  type    = string
  default = "bnadmin"
}

# Forneca via TF_VAR_db_master_password. Ephemeral + password_wo impedem que
# o valor seja persistido no plan/state (Terraform >= 1.11 + AWS provider 6.x).
variable "db_master_password" {
  type      = string
  sensitive = true
  ephemeral = true
}

variable "db_master_password_version" {
  description = "Incremente somente quando rotacionar a senha master"
  type        = number
  default     = 1
}

variable "budget_email" {
  description = "E-mail que recebera alertas de custo"
  type        = string
}

variable "monthly_budget_usd" {
  type    = number
  default = 10
}
