variable "project_name" { type = string }
variable "environment" { type = string }
variable "aws_account_id" { type = string }

variable "enabled" {
  description = "false pausa a agenda (schedules ficam DISABLED) sem destruir nada"
  type        = bool
  default     = true
}

variable "timezone" {
  description = "Fuso IANA das expressoes cron (o Scheduler trata horario de verao)"
  type        = string
  default     = "America/Sao_Paulo"
}

# Expressoes no formato do EventBridge: cron(min hora dia-do-mes mes dia-da-semana ano)
variable "schedule" {
  type = object({
    rds_start = string
    ec2_start = string
    ec2_stop  = string
    rds_stop  = string
  })
}

variable "instance_id" { type = string }
variable "instance_arn" { type = string }
variable "db_instance_identifier" { type = string }
variable "db_instance_arn" { type = string }
