# Security Group da EC2 SEM nenhuma regra de entrada (ADR-012).
# - API: chega pelo Cloudflare Tunnel, uma conexao de SAIDA aberta pelo
#   cloudflared. Scanner nenhum encontra porta aberta, nem do CloudFront.
# - Administracao: SSM Session Manager, tambem so conexao de saida (sem SSH).
resource "aws_security_group" "ec2" {
  name        = "${var.project_name}-${var.environment}-ec2-sg"
  description = "Sem entrada: API via Cloudflare Tunnel, gestao via SSM"
  vpc_id      = var.vpc_id

  egress {
    description = "Saida livre: Cloudflare Tunnel, ECR, SSM, SQS/SNS, S3 e RDS"
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name        = "${var.project_name}-${var.environment}-ec2-sg"
    Environment = var.environment
  }
}

resource "aws_security_group" "rds" {
  name        = "${var.project_name}-${var.environment}-rds-sg"
  description = "PostgreSQL acessivel somente pela EC2 do BnShop"
  vpc_id      = var.vpc_id

  ingress {
    description     = "PostgreSQL vindo da EC2"
    from_port       = 5432
    to_port         = 5432
    protocol        = "tcp"
    security_groups = [aws_security_group.ec2.id]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "${var.project_name}-${var.environment}-rds-sg"
  }
}

# Trust policy da IAM Role: apenas o servico EC2 pode assumi-la.
data "aws_iam_policy_document" "ec2_assume_role" {
  statement {
    actions = ["sts:AssumeRole"]

    principals {
      type        = "Service"
      identifiers = ["ec2.amazonaws.com"]
    }
  }
}

resource "aws_iam_role" "ec2" {
  name               = "${var.project_name}-${var.environment}-ec2-role"
  assume_role_policy = data.aws_iam_policy_document.ec2_assume_role.json

  tags = {
    Environment = var.environment
  }
}

# Policy gerenciada da AWS que habilita o SSM Session Manager
# (terminal remoto sem SSH e sem porta 22 aberta).
resource "aws_iam_role_policy_attachment" "ssm_core" {
  role       = aws_iam_role.ec2.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonSSMManagedInstanceCore"
}

data "aws_iam_policy_document" "application_runtime" {
  statement {
    sid       = "ReadStagingParameters"
    actions   = ["ssm:GetParameter", "ssm:GetParameters"]
    resources = ["arn:aws:ssm:${var.aws_region}:${var.aws_account_id}:parameter${var.parameter_path_prefix}/*"]
  }

  statement {
    sid = "CatalogBucket"
    actions = [
      "s3:ListBucket"
    ]
    resources = [var.catalog_bucket_arn]
  }

  statement {
    sid = "CatalogObjects"
    actions = [
      "s3:GetObject", "s3:PutObject", "s3:DeleteObject"
    ]
    resources = ["${var.catalog_bucket_arn}/*"]
  }

  statement {
    sid       = "ReadPrivateArtifacts"
    actions   = ["s3:GetObject"]
    resources = ["${var.private_bucket_arn}/efi/*"]
  }

  # CreateQueue/CreateTopic precisam de Resource="*" porque o ARN ainda nao
  # existe. As demais operacoes ficam limitadas ao prefixo de staging.
  statement {
    sid       = "CreateMassTransitTopology"
    actions   = ["sqs:CreateQueue", "sns:CreateTopic"]
    resources = ["*"]
  }

  statement {
    sid = "UseMassTransitQueues"
    actions = [
      "sqs:DeleteMessage", "sqs:GetQueueAttributes", "sqs:GetQueueUrl",
      "sqs:ReceiveMessage", "sqs:SendMessage", "sqs:SetQueueAttributes",
      "sqs:TagQueue"
    ]
    resources = ["arn:aws:sqs:${var.aws_region}:${var.aws_account_id}:${var.environment}-*"]
  }

  statement {
    sid = "UseMassTransitTopics"
    actions = [
      "sns:GetTopicAttributes", "sns:ListSubscriptionsByTopic", "sns:Publish",
      "sns:SetTopicAttributes", "sns:Subscribe", "sns:TagResource", "sns:Unsubscribe"
    ]
    resources = ["arn:aws:sns:${var.aws_region}:${var.aws_account_id}:${var.environment}-*"]
  }

  # GetAuthorizationToken nao suporta permissao por recurso.
  statement {
    sid       = "EcrLogin"
    actions   = ["ecr:GetAuthorizationToken"]
    resources = ["*"]
  }

  statement {
    sid = "EcrPull"
    actions = [
      "ecr:BatchCheckLayerAvailability", "ecr:BatchGetImage",
      "ecr:GetDownloadUrlForLayer"
    ]
    resources = var.ecr_repository_arns
  }

  # O pipeline publica docker-compose + deploy.sh versionados por commit em
  # s3://<private>/deploy/<sha>/. A EC2 so le esse prefixo.
  statement {
    sid       = "ReadDeployBundles"
    actions   = ["s3:GetObject"]
    resources = ["${var.private_bucket_arn}/deploy/*"]
  }

  # "aws s3 cp --recursive" baixa a PASTA do commit: listar o bucket e uma
  # permissao do BUCKET (nao do objeto). Restrita ao prefixo deploy/.
  statement {
    sid       = "ListDeployBundles"
    actions   = ["s3:ListBucket"]
    resources = [var.private_bucket_arn]

    condition {
      test     = "StringLike"
      variable = "s3:prefix"
      values   = ["deploy/*"]
    }
  }
}

resource "aws_iam_role_policy" "application_runtime" {
  name   = "${var.project_name}-${var.environment}-runtime"
  role   = aws_iam_role.ec2.id
  policy = data.aws_iam_policy_document.application_runtime.json
}

# Instance profile: e o que efetivamente "anexa" a role a uma EC2.
resource "aws_iam_instance_profile" "ec2" {
  name = "${var.project_name}-${var.environment}-ec2-profile"
  role = aws_iam_role.ec2.name
}
