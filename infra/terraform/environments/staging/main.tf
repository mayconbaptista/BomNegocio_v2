data "aws_caller_identity" "current" {}

locals {
  environment           = "staging"
  parameter_path_prefix = "/${var.project_name}/${local.environment}"
}

module "budget" {
  source = "../../modules/budget"

  project_name = var.project_name
  environment  = local.environment
  limit_usd    = var.monthly_budget_usd
  email        = var.budget_email
}

module "network" {
  source = "../../modules/network"

  project_name            = var.project_name
  environment             = local.environment
  vpc_cidr                = var.vpc_cidr
  public_subnet_cidr      = var.public_subnet_cidr
  private_db_subnet_cidrs = var.private_db_subnet_cidrs
  availability_zones      = var.availability_zones
}

module "storage" {
  source = "../../modules/storage"

  project_name   = var.project_name
  environment    = local.environment
  aws_account_id = data.aws_caller_identity.current.account_id
}

module "security" {
  source = "../../modules/security"

  project_name          = var.project_name
  environment           = local.environment
  vpc_id                = module.network.vpc_id
  aws_region            = var.aws_region
  aws_account_id        = data.aws_caller_identity.current.account_id
  parameter_path_prefix = local.parameter_path_prefix
  catalog_bucket_arn    = module.storage.catalog_bucket_arn
  private_bucket_arn    = module.storage.private_bucket_arn
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

# Configuracoes nao sensiveis podem ser administradas pelo Terraform.
resource "aws_ssm_parameter" "aws_region" {
  name  = "${local.parameter_path_prefix}/config/aws-region"
  type  = "String"
  tier  = "Standard"
  value = var.aws_region
}

resource "aws_ssm_parameter" "catalog_bucket" {
  name  = "${local.parameter_path_prefix}/config/catalog-bucket"
  type  = "String"
  tier  = "Standard"
  value = module.storage.catalog_bucket_name
}

resource "aws_ssm_parameter" "rds_endpoint" {
  name  = "${local.parameter_path_prefix}/config/rds-endpoint"
  type  = "String"
  tier  = "Standard"
  value = module.database.endpoint
}
