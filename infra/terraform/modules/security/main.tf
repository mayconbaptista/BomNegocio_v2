# Security Group da EC2. So HTTPS (443) e liberado para a internet.
# A porta 80 fica fechada porque a validacao do certificado usa o
# desafio DNS-01 do Let's Encrypt (via API do Cloudflare), nao HTTP-01
# - entao nao ha necessidade de expor a 80 so para o ACME responder.
# SSH (22) tambem nao e liberado: o acesso administrativo e via SSM
# Session Manager, que usa apenas conexao de SAIDA (egress), entao nao
# precisa de nenhuma porta de entrada aberta para gestao da instancia.
resource "aws_security_group" "ec2" {
  name        = "${var.project_name}-${var.environment}-ec2-sg"
  description = "Permite apenas HTTPS de entrada; gestao via SSM Session Manager"
  vpc_id      = var.vpc_id

  ingress {
    description = "HTTPS publico (Gateway atras do Traefik)"
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    description = "Saida livre: pull de imagens, SSM, API do Cloudflare, etc"
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
