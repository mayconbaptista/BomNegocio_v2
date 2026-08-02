terraform {
  required_version = ">= 1.11, < 2.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 6.0"
    }
  }

  # Estado local por enquanto. Antes de CI/CD, migrar para backend S3 com
  # encrypt=true, bucket versionado e use_lockfile=true. O locking via
  # DynamoDB foi depreciado pelo Terraform e nao deve ser adotado em projeto novo.
}

provider "aws" {
  region = var.aws_region

  default_tags {
    tags = {
      Project     = var.project_name
      Environment = "staging"
      ManagedBy   = "terraform"
    }
  }
}
