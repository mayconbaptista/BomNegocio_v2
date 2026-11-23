variable "project_name" {
  description = "Prefixo usado em nomes e tags"
  type        = string
  default     = "bnshop"
}

# Regiao do projeto AWS (nova experiencia): a SCP gerenciada NEGA recursos
# regionais em qualquer outra. Confira em AWS Settings > View all projects >
# Overview > Additional Info > Region. As zonas sao derivadas dela (main.tf).
variable "aws_region" {
  type    = string
  default = "us-east-2"
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

variable "alert_email" {
  description = "E-mail para alertas de custo (Budget) e operacao (CloudWatch)"
  type        = string
}

# Teto para os creditos durarem os 6 meses do Free Plan: US$ 100 / 6 = 16,6.
# Esperado com dias uteis 8h-20h + Tunnel: ~US$ 14/mes (Plano de Custos).
# Acima de 16, os creditos acabam antes do mes 6: agenda desligada, recurso
# esquecido, ambiente ligado fora do horario...
variable "monthly_budget_usd" {
  type    = number
  default = 16
}

variable "auth_valid_issuer" {
  type    = string
  default = "bn-auth-sso"
}

variable "github_backend_repository" {
  description = "owner/repo do backend .NET"
  type        = string
  default     = "mayconbaptista/BomNegocio_v2"
}

variable "github_frontend_repository" {
  description = "owner/repo do frontend Angular"
  type        = string
  default     = "mayconbaptista/bnEshop-ecommerce"
}

# --- Dominio (Cloudflare somente DNS -> CloudFront) --------------------------

variable "cloudflare_account_id" {
  description = "Account ID da Cloudflare (painel do dominio > Overview > API); usado pelo Tunnel"
  type        = string
}

variable "cloudflare_zone_id" {
  description = "Zone ID do dominio no Cloudflare (painel do dominio > Overview > API)"
  type        = string
}

variable "domain_name" {
  description = "Dominio raiz registrado no Cloudflare, ex.: meudominio.com.br"
  type        = string
}

variable "app_subdomain" {
  description = "Subdominio do BnEshop; string vazia usa o dominio raiz"
  type        = string
  default     = "bneshop"
}

# --- Agenda liga/desliga (EventBridge Scheduler) ------------------------------

variable "power_schedule_enabled" {
  description = "false = ambiente 24/7 (schedules ficam DISABLED, nada e destruido)"
  type        = bool
  default     = true
}

variable "power_schedule_timezone" {
  type    = string
  default = "America/Sao_Paulo"
}

# cron(min hora dia-do-mes mes dia-da-semana ano), no fuso acima.
# Liga em dias uteis 8h-20h (horario em que recrutadores costumam abrir o
# link). Os desligamentos rodam TODO dia as 20h e as 23h: se voce ligar
# manualmente (post no LinkedIn, entrevista), o ambiente nao fica esquecido.
# Parar algo que ja esta parado gera um erro inofensivo no Scheduler.
variable "power_schedule" {
  type = object({
    rds_start = string
    ec2_start = string
    ec2_stop  = string
    rds_stop  = string
  })
  default = {
    rds_start = "cron(40 7 ? * MON-FRI *)" # 20 min antes: o RDS leva 5-10 min para ficar "available"
    ec2_start = "cron(0 8 ? * MON-FRI *)"
    ec2_stop  = "cron(0 20,23 * * ? *)"
    rds_stop  = "cron(10 20,23 * * ? *)" # depois da app, para nao derrubar conexoes abertas
  }
}
