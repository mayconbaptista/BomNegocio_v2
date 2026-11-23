terraform {
  required_version = ">= 1.11, < 2.0"

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 6.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 3.6"
    }
    cloudflare = {
      source  = "cloudflare/cloudflare"
      version = "~> 5.0"
    }
  }

  # Remote state no S3 com lock nativo (use_lockfile). Os valores ficam em
  # backend.hcl (nao versionado), criado a partir de backend.hcl.example
  # depois de aplicar infra/terraform/bootstrap:
  #   terraform init -backend-config=backend.hcl
  backend "s3" {}
}

locals {
  default_tags = {
    Project     = var.project_name
    Environment = "staging"
    ManagedBy   = "terraform"
  }
}

provider "aws" {
  region = var.aws_region

  default_tags {
    tags = local.default_tags
  }
}

# Regiao FIXA, nao confundir com var.aws_region (a regiao do projeto,
# us-east-2). O CloudFront e global, mas so le certificados do ACM em
# us-east-1 (N. Virginia): regra da AWS, nao escolha nossa. Este alias existe
# para criar UM recurso (o certificado) fora da regiao do resto da stack.
# A SCP do projeto libera acm:* e cloudfront:* em us-east-1.
provider "aws" {
  alias  = "us_east_1"
  region = "us-east-1"

  default_tags {
    tags = local.default_tags
  }
}

# Token lido de CLOUDFLARE_API_TOKEN (nunca em tfvars nem no state).
# Permissoes minimas: Account > Cloudflare Tunnel > Edit + Zone > DNS > Edit
# (restrita a zona do dominio).
provider "cloudflare" {}
