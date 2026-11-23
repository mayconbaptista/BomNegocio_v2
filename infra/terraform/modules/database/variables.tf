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


# false = destroy gera snapshot final (custo de storage de snapshot, mas
# protege os dados). Use true somente em ambiente sabidamente descartavel.
variable "skip_final_snapshot" {
  type    = bool
  default = false
}

# Formato UTC "hh24:mi-hh24:mi". null = AWS escolhe (pode cair com o banco
# desligado se houver agenda de liga/desliga).
variable "backup_window" {
  type    = string
  default = null
}

# Formato UTC "ddd:hh24:mi-ddd:hh24:mi"; nao pode sobrepor o backup_window.
variable "maintenance_window" {
  type    = string
  default = null
}
