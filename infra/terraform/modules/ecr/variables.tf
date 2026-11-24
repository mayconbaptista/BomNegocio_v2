variable "project_name" { type = string }

variable "services" {
  type    = list(string)
  default = ["gateway", "auth", "cart", "catalog", "order"]
}

variable "images_to_keep" {
  type    = number
  default = 2
}

variable "force_delete" {
  description = "Permite destroy com imagens dentro. Conveniente em estudo."
  type        = bool
  default     = true
}
