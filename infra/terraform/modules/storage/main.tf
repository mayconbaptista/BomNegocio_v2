locals {
  prefix = "${var.project_name}-${var.environment}-${var.aws_account_id}"
}

resource "aws_s3_bucket" "catalog" {
  bucket        = "${local.prefix}-catalog"
  force_destroy = false
}

resource "aws_s3_bucket" "private_artifacts" {
  bucket        = "${local.prefix}-private"
  force_destroy = false
}

resource "aws_s3_bucket_public_access_block" "all" {
  for_each = {
    catalog = aws_s3_bucket.catalog.id
    private = aws_s3_bucket.private_artifacts.id
  }

  bucket                  = each.value
  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}

resource "aws_s3_bucket_server_side_encryption_configuration" "all" {
  for_each = {
    catalog = aws_s3_bucket.catalog.id
    private = aws_s3_bucket.private_artifacts.id
  }

  bucket = each.value

  rule {
    apply_server_side_encryption_by_default {
      sse_algorithm = "AES256"
    }
  }
}

resource "aws_s3_bucket_lifecycle_configuration" "catalog" {
  bucket = aws_s3_bucket.catalog.id

  rule {
    id     = "abort-incomplete-multipart-uploads"
    status = "Enabled"

    abort_incomplete_multipart_upload {
      days_after_initiation = 7
    }
  }
}

