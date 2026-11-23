output "public_url" {
  description = "URL publica: Angular em /, API em /api, imagens em /media"
  value       = module.edge.public_url
}

output "cloudfront_domain_name" {
  description = "Continua respondendo; util para testar antes da propagacao do DNS"
  value       = module.edge.domain_name
}

output "instance_id" { value = module.compute.instance_id }
output "api_origin_hostname" { value = module.tunnel.origin_hostname }
output "rds_endpoint" { value = module.database.endpoint }
output "rds_instance_id" { value = module.database.identifier }
output "catalog_bucket" { value = module.storage.catalog_bucket_name }
output "private_bucket" { value = module.storage.private_bucket_name }
output "spa_bucket" { value = module.edge.spa_bucket_name }
output "cloudfront_distribution_id" { value = module.edge.distribution_id }
output "ecr_repository_urls" { value = module.ecr.repository_urls }
output "parameter_path_prefix" { value = local.parameter_path_prefix }
output "power_schedule_group" { value = module.power_schedule.schedule_group_name }

# Usuarios IAM do GitHub Actions. As access keys sao geradas FORA do Terraform:
#   infra/scripts/New-GitHubDeployKey.ps1 -UserName <valor abaixo>
output "github_backend_user" { value = module.cicd.backend_user_name }
output "github_frontend_user" { value = module.cicd.frontend_user_name }
