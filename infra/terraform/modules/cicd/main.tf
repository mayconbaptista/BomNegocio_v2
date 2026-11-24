# GitHub Actions -> AWS via usuario IAM "de maquina" + access key.
#
# Por que nao OIDC: na nova experiencia da AWS (Free Tier), a SCP gerenciada
# nega iam:*Provider* (Sid DenyIAMRestrictedActions), entao o
# aws_iam_openid_connect_provider do GitHub nao pode ser criado (ADR-015).
#
# Mitigacoes de uma chave de longa duracao:
#   - um usuario por repositorio, cada um com o minimo necessario;
#   - sem senha de console (login profile e negado pela SCP de qualquer forma);
#   - a access key NAO e criada aqui: o script infra/scripts/New-GitHubDeployKey.ps1
#     gera a chave e copia o segredo direto para a area de transferencia, sem
#     passar pelo state do Terraform nem pela tela;
#   - rotacao: rodar o script de novo (apaga a chave anterior).
#
# Quando o projeto puder usar OIDC (advanced features), volta-se a role com
# trust no token.actions.githubusercontent.com e as mesmas politicas abaixo.

locals {
  name = "${var.project_name}-${var.environment}"
}

# ---------------------------------------------------------------------------
# Backend: push no ECR, publica bundle de deploy no S3, dispara SSM Run Command
# ---------------------------------------------------------------------------
resource "aws_iam_user" "backend_deploy" {
  name = "${local.name}-gha-backend"
  path = "/ci/"

  tags = {
    Repository = var.backend_repository
  }
}

data "aws_iam_policy_document" "backend_deploy" {
  statement {
    sid       = "EcrLogin"
    actions   = ["ecr:GetAuthorizationToken"]
    resources = ["*"]
  }

  statement {
    sid = "EcrPush"
    actions = [
      "ecr:BatchCheckLayerAvailability", "ecr:BatchGetImage",
      "ecr:CompleteLayerUpload", "ecr:DescribeImages",
      "ecr:GetDownloadUrlForLayer", "ecr:InitiateLayerUpload",
      "ecr:PutImage", "ecr:UploadLayerPart"
    ]
    resources = var.ecr_repository_arns
  }

  statement {
    sid       = "UploadDeployBundle"
    actions   = ["s3:PutObject"]
    resources = ["${var.private_bucket_arn}/deploy/*"]
  }

  # SendCommand exige permissao tanto na instancia quanto no documento.
  statement {
    sid     = "RunDeployOnInstance"
    actions = ["ssm:SendCommand"]
    resources = [
      var.instance_arn,
      "arn:aws:ssm:${var.aws_region}::document/AWS-RunShellScript"
    ]
  }

  statement {
    sid       = "ReadCommandResult"
    actions   = ["ssm:GetCommandInvocation", "ssm:ListCommandInvocations"]
    resources = ["*"]
  }

  # Workflow "Power staging" (liga/desliga manual) e checagem pre-deploy.
  # Describe* nao suporta restricao por recurso no EC2/SSM, por isso "*".
  statement {
    sid = "ReadPowerState"
    actions = [
      "ec2:DescribeInstances",
      "ec2:DescribeInstanceStatus",
      "ssm:DescribeInstanceInformation"
    ]
    resources = ["*"]
  }

  statement {
    sid       = "PowerEc2"
    actions   = ["ec2:StartInstances", "ec2:StopInstances"]
    resources = [var.instance_arn]
  }

  statement {
    sid       = "PowerRds"
    actions   = ["rds:StartDBInstance", "rds:StopDBInstance", "rds:DescribeDBInstances"]
    resources = [var.db_instance_arn]
  }
}

resource "aws_iam_user_policy" "backend_deploy" {
  name   = "deploy"
  user   = aws_iam_user.backend_deploy.name
  policy = data.aws_iam_policy_document.backend_deploy.json
}

# ---------------------------------------------------------------------------
# Frontend: sync do build no bucket da SPA + invalidacao do index.html
# ---------------------------------------------------------------------------
resource "aws_iam_user" "frontend_deploy" {
  name = "${local.name}-gha-frontend"
  path = "/ci/"

  tags = {
    Repository = var.frontend_repository
  }
}

data "aws_iam_policy_document" "frontend_deploy" {
  statement {
    sid       = "ListSpaBucket"
    actions   = ["s3:ListBucket"]
    resources = [var.spa_bucket_arn]
  }

  statement {
    sid       = "SyncSpaObjects"
    actions   = ["s3:PutObject", "s3:DeleteObject", "s3:GetObject"]
    resources = ["${var.spa_bucket_arn}/*"]
  }

  statement {
    sid       = "InvalidateCdn"
    actions   = ["cloudfront:CreateInvalidation", "cloudfront:GetInvalidation"]
    resources = [var.distribution_arn]
  }
}

resource "aws_iam_user_policy" "frontend_deploy" {
  name   = "deploy"
  user   = aws_iam_user.frontend_deploy.name
  policy = data.aws_iam_policy_document.frontend_deploy.json
}
