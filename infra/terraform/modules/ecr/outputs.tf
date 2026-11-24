output "repository_urls" {
  value = { for k, r in aws_ecr_repository.service : k => r.repository_url }
}

output "repository_arns" {
  value = [for r in aws_ecr_repository.service : r.arn]
}

output "registry" {
  value = split("/", values(aws_ecr_repository.service)[0].repository_url)[0]
}
