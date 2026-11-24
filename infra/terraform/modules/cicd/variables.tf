variable "project_name" { type = string }
variable "environment" { type = string }
variable "aws_region" { type = string }
variable "aws_account_id" { type = string }

variable "backend_repository" {
  description = "owner/repo do backend no GitHub"
  type        = string
}

variable "frontend_repository" {
  description = "owner/repo do frontend no GitHub"
  type        = string
}

variable "ecr_repository_arns" { type = list(string) }
variable "private_bucket_arn" { type = string }
variable "instance_arn" { type = string }
variable "spa_bucket_arn" { type = string }
variable "distribution_arn" { type = string }
variable "db_instance_arn" { type = string }
