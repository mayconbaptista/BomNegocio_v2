module "network" {
  source = "../../modules/network"

  project_name        = var.project_name
  environment          = "staging"
  vpc_cidr             = var.vpc_cidr
  public_subnet_cidr  = var.public_subnet_cidr
  availability_zone    = var.availability_zone
}

module "security" {
  source = "../../modules/security"

  project_name          = var.project_name
  environment           = "staging"
  vpc_id                = module.network.vpc_id
  cloudflare_api_token  = var.cloudflare_api_token
}
