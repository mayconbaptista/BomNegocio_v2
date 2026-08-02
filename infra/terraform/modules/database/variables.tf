variable "project_name" { type = string }
variable "environment" { type = string }
variable "subnet_ids" { type = list(string) }
variable "security_group_id" { type = string }
variable "instance_class" { type = string }
variable "allocated_storage" { type = number }
variable "master_username" { type = string }
variable "master_password" {
  type      = string
  sensitive = true
  ephemeral = true
}
variable "master_password_version" { type = number }

