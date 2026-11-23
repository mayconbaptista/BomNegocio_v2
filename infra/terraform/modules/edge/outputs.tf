output "distribution_id" { value = aws_cloudfront_distribution.main.id }
output "distribution_arn" { value = aws_cloudfront_distribution.main.arn }
output "domain_name" { value = aws_cloudfront_distribution.main.domain_name }
output "public_url" {
  description = "Dominio proprio quando configurado; senao o *.cloudfront.net"
  value       = "https://${length(var.aliases) > 0 ? var.aliases[0] : aws_cloudfront_distribution.main.domain_name}"
}
output "spa_bucket_name" { value = aws_s3_bucket.spa.id }
output "spa_bucket_arn" { value = aws_s3_bucket.spa.arn }
