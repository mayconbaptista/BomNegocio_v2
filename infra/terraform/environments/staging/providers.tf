terraform {
  required_version = ">= 1.5"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }

  # Estado local por enquanto (suficiente para estudo/single-user).
  # Se no futuro mais de uma pessoa/maquina for aplicar este Terraform,
  # trocar para um backend remoto (S3 + DynamoDB lock) evita que dois
  # applies simultaneos corrompam o state.
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
