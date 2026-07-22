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

# Parametro SecureString com o token do Cloudflare. E criptografado com
# a KMS key default da conta - o valor nunca aparece em texto plano no
# state nem em nenhum output. O Traefik le esse valor no boot da EC2.
resource "aws_ssm_parameter" "cloudflare_api_token" {
  name  = "/${var.project_name}/${var.environment}/cloudflare_api_token"
  type  = "SecureString"
  value = var.cloudflare_api_token

  tags = {
    Environment = var.environment
  }
}

# Policy minima: a role so pode ler ESSE parametro especifico, nunca
# qualquer parametro da conta (principio do menor privilegio).
data "aws_iam_policy_document" "read_cloudflare_token" {
  statement {
    actions   = ["ssm:GetParameter"]
    resources = [aws_ssm_parameter.cloudflare_api_token.arn]
  }
}

resource "aws_iam_role_policy" "read_cloudflare_token" {
  name   = "${var.project_name}-${var.environment}-read-cloudflare-token"
  role   = aws_iam_role.ec2.id
  policy = data.aws_iam_policy_document.read_cloudflare_token.json
}

# Instance profile: e o que efetivamente "anexa" a role a uma EC2.
resource "aws_iam_instance_profile" "ec2" {
  name = "${var.project_name}-${var.environment}-ec2-profile"
  role = aws_iam_role.ec2.name
}
