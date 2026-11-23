data "aws_caller_identity" "current" {}

# Zonas perguntadas a AWS em vez de escritas a mao: trocar a regiao do
# provider basta para as sub-redes acompanharem (uma fonte da verdade).
data "aws_availability_zones" "available" {
  state = "available"
}

locals {
  environment           = "staging"
  account_id            = data.aws_caller_identity.current.account_id
  azs                   = slice(data.aws_availability_zones.available.names, 0, 2)
  parameter_path_prefix = "/${var.project_name}/${local.environment}"

  # Origem da API via Cloudflare Tunnel. Precisa ser subdominio de PRIMEIRO
  # nivel (coberto pelo certificado gratuito *.dominio da Cloudflare).
  origin_hostname = var.app_subdomain == "" ? "origin.${var.domain_name}" : "${var.app_subdomain}-origin.${var.domain_name}"
}

# Fase 0 - guardrail de custo antes de qualquer recurso pago.
module "budget" {
  source = "../../modules/budget"

  project_name = var.project_name
  environment  = local.environment
  limit_usd    = var.monthly_budget_usd
  email        = var.alert_email
}

module "network" {
  source = "../../modules/network"

  project_name            = var.project_name
  environment             = local.environment
  vpc_cidr                = var.vpc_cidr
  public_subnet_cidr      = var.public_subnet_cidr
  private_db_subnet_cidrs = var.private_db_subnet_cidrs
  availability_zones      = local.azs
}

module "storage" {
  source = "../../modules/storage"

  project_name   = var.project_name
  environment    = local.environment
  aws_account_id = local.account_id
}

module "ecr" {
  source = "../../modules/ecr"

  project_name = var.project_name
}

module "security" {
  source = "../../modules/security"

  project_name          = var.project_name
  environment           = local.environment
  vpc_id                = module.network.vpc_id
  aws_region            = var.aws_region
  aws_account_id        = local.account_id
  parameter_path_prefix = local.parameter_path_prefix
  catalog_bucket_arn    = module.storage.catalog_bucket_arn
  private_bucket_arn    = module.storage.private_bucket_arn
  ecr_repository_arns   = module.ecr.repository_arns
}

module "database" {
  source = "../../modules/database"

  project_name            = var.project_name
  environment             = local.environment
  subnet_ids              = module.network.database_subnet_ids
  security_group_id       = module.security.rds_security_group_id
  instance_class          = var.db_instance_class
  allocated_storage       = var.db_allocated_storage
  master_username         = var.db_master_username
  master_password         = var.db_master_password
  master_password_version = var.db_master_password_version

  # UTC. Brasilia = UTC-3: backup 11:00-11:30 e manutencao quarta 12:00,
  # dentro da janela em que a agenda mantem o banco ligado (dias uteis 8h-20h).
  # Fim de semana o banco fica parado, sem backup, e sem dados novos tambem.
  backup_window      = "14:00-14:30"
  maintenance_window = "wed:15:00-wed:15:30"
}

module "compute" {
  source = "../../modules/compute"

  project_name              = var.project_name
  environment               = local.environment
  subnet_id                 = module.network.public_subnet_id
  security_group_id         = module.security.security_group_id
  iam_instance_profile_name = module.security.iam_instance_profile_name
  instance_type             = var.instance_type
  architecture              = var.instance_architecture
  root_volume_size          = var.root_volume_size
  docker_compose_version    = var.docker_compose_version
}

# Segredo compartilhado CloudFront -> Traefik. Ele precisa existir na config
# da distribuicao, entao estara no state de qualquer forma; por isso e gerado
# aqui (diferente das senhas de banco, que nunca passam pelo Terraform).
resource "random_password" "origin_verify" {
  length  = 48
  special = false
}

module "edge" {
  source = "../../modules/edge"

  project_name                        = var.project_name
  environment                         = local.environment
  aws_account_id                      = local.account_id
  api_origin_domain                   = module.tunnel.origin_hostname
  origin_verify_secret                = random_password.origin_verify.result
  catalog_bucket_id                   = module.storage.catalog_bucket_name
  catalog_bucket_arn                  = module.storage.catalog_bucket_arn
  catalog_bucket_regional_domain_name = module.storage.catalog_bucket_regional_domain_name
  aliases                             = [module.custom_domain.fqdn]
  acm_certificate_arn                 = module.custom_domain.certificate_arn
}

# Dominio do Cloudflare (somente DNS) apontando para o CloudFront, com
# certificado ACM. Os dois modulos se referenciam, mas sem ciclo real:
# certificado -> distribuicao -> CNAME.
module "custom_domain" {
  source = "../../modules/custom-domain"

  providers = {
    aws.us_east_1 = aws.us_east_1
  }

  environment            = local.environment
  cloudflare_zone_id     = var.cloudflare_zone_id
  domain_name            = var.domain_name
  subdomain              = var.app_subdomain
  cloudfront_domain_name = module.edge.domain_name
}

# Cloudflare Tunnel: a API chega na EC2 sem porta aberta e sem IP fixo
# (ADR-012).
module "tunnel" {
  source = "../../modules/tunnel"

  project_name          = var.project_name
  environment           = local.environment
  cloudflare_account_id = var.cloudflare_account_id
  cloudflare_zone_id    = var.cloudflare_zone_id
  origin_hostname       = local.origin_hostname
}

module "cicd" {
  source = "../../modules/cicd"

  project_name        = var.project_name
  environment         = local.environment
  aws_region          = var.aws_region
  aws_account_id      = local.account_id
  backend_repository  = var.github_backend_repository
  frontend_repository = var.github_frontend_repository
  ecr_repository_arns = module.ecr.repository_arns
  private_bucket_arn  = module.storage.private_bucket_arn
  instance_arn        = module.compute.instance_arn
  db_instance_arn     = module.database.arn
  spa_bucket_arn      = module.edge.spa_bucket_arn
  distribution_arn    = module.edge.distribution_arn
}

# Liga/desliga por horario (ADR-008). Sem Lambda: o Scheduler chama a API
# da AWS direto.
module "power_schedule" {
  source = "../../modules/power-schedule"

  project_name           = var.project_name
  environment            = local.environment
  aws_account_id         = local.account_id
  enabled                = var.power_schedule_enabled
  timezone               = var.power_schedule_timezone
  schedule               = var.power_schedule
  instance_id            = module.compute.instance_id
  instance_arn           = module.compute.instance_arn
  db_instance_identifier = module.database.identifier
  db_instance_arn        = module.database.arn
}

# Disjuntor de creditos (ADR-013): estourou o limite mensal, desliga e
# bloqueia o religamento ate o mes seguinte.
module "cost_guard" {
  source = "../../modules/cost-guard"

  project_name           = var.project_name
  environment            = local.environment
  aws_region             = var.aws_region
  budget_name            = module.budget.name
  alert_email            = var.alert_email
  instance_id            = module.compute.instance_id
  db_instance_identifier = module.database.identifier
  roles_to_block         = [module.power_schedule.scheduler_role_name]
  users_to_block         = [module.cicd.backend_user_name]
}

module "observability" {
  source = "../../modules/observability"

  project_name  = var.project_name
  environment   = local.environment
  aws_region    = var.aws_region
  alert_email   = var.alert_email
  instance_id   = module.compute.instance_id
  db_identifier = module.database.identifier
}

# ---------------------------------------------------------------------------
# Parameter Store (Standard tier = gratuito)
# config/*  -> String, gerenciado aqui
# secrets/* -> SecureString; senhas sao gravadas FORA do Terraform pelo
#              script infra/scripts/Set-StagingSecrets.ps1 (nao vao ao state)
# ---------------------------------------------------------------------------
locals {
  config_parameters = {
    "aws-region"          = var.aws_region
    "catalog-bucket"      = module.storage.catalog_bucket_name
    "private-bucket"      = module.storage.private_bucket_name
    "rds-endpoint"        = module.database.endpoint
    "ecr-registry"        = module.ecr.registry
    "public-url"          = module.edge.public_url
    "auth-valid-audience" = module.edge.public_url
    "auth-valid-issuer"   = var.auth_valid_issuer
  }
}

resource "aws_ssm_parameter" "config" {
  for_each = local.config_parameters

  name  = "${local.parameter_path_prefix}/config/${each.key}"
  type  = "String"
  tier  = "Standard"
  value = each.value
}

resource "aws_ssm_parameter" "origin_verify_secret" {
  name  = "${local.parameter_path_prefix}/secrets/origin-verify-secret"
  type  = "SecureString" # alias/aws/ssm: sem custo mensal de chave
  tier  = "Standard"
  value = random_password.origin_verify.result
}

# Token do cloudflared. Vem de um data source da Cloudflare, entao ja esta no
# state de qualquer forma (mesma excecao consciente do origin-verify). O token
# so permite RODAR este tunnel: nao edita DNS nem outros recursos da conta.
resource "aws_ssm_parameter" "cloudflare_tunnel_token" {
  name  = "${local.parameter_path_prefix}/secrets/cloudflare-tunnel-token"
  type  = "SecureString"
  tier  = "Standard"
  value = module.tunnel.token
}
