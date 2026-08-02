variable "project_name" { type = string }
variable "environment" { type = string }
variable "subnet_id" { type = string }
variable "security_group_id" { type = string }
variable "iam_instance_profile_name" { type = string }
variable "instance_type" { type = string }
variable "architecture" {
  type = string
  validation {
    condition     = contains(["arm64", "x86_64"], var.architecture)
    error_message = "architecture deve ser arm64 ou x86_64."
  }
}
variable "root_volume_size" { type = number }
variable "docker_compose_version" { type = string }

