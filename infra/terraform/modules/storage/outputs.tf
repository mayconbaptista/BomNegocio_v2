output "catalog_bucket_name" { value = aws_s3_bucket.catalog.id }
output "catalog_bucket_arn" { value = aws_s3_bucket.catalog.arn }
output "private_bucket_name" { value = aws_s3_bucket.private_artifacts.id }
output "private_bucket_arn" { value = aws_s3_bucket.private_artifacts.arn }

