variable "project_name" { type = string }
variable "environment" { type = string }
variable "aws_region" { type = string }

variable "budget_name" {
  description = "Budget mensal (custo bruto) que dispara o disjuntor"
  type        = string
}

variable "threshold_percent" {
  description = "Percentual do limite mensal em que o ambiente e desligado e bloqueado"
  type        = number
  default     = 100
}

variable "alert_email" { type = string }
variable "instance_id" { type = string }
variable "db_instance_identifier" { type = string }

variable "roles_to_block" {
  description = "Roles que podem LIGAR o ambiente (Scheduler)"
  type        = list(string)
}

variable "users_to_block" {
  description = "Usuarios IAM de maquina que podem LIGAR o ambiente (GitHub Actions)"
  type        = list(string)
  default     = []
}
