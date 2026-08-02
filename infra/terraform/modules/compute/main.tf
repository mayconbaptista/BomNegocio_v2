locals {
  ami_parameter = var.architecture == "arm64" ? "/aws/service/ami-amazon-linux-latest/al2023-ami-kernel-default-arm64" : "/aws/service/ami-amazon-linux-latest/al2023-ami-kernel-default-x86_64"
}

data "aws_ssm_parameter" "amazon_linux_2023" {
  name = local.ami_parameter
}

resource "aws_instance" "app" {
  ami                         = data.aws_ssm_parameter.amazon_linux_2023.value
  instance_type               = var.instance_type
  subnet_id                   = var.subnet_id
  vpc_security_group_ids      = [var.security_group_id]
  iam_instance_profile        = var.iam_instance_profile_name
  associate_public_ip_address = true
  monitoring                  = false

  metadata_options {
    http_endpoint = "enabled"
    http_tokens   = "required"
  }

  root_block_device {
    volume_type           = "gp3"
    volume_size           = var.root_volume_size
    encrypted             = true
    delete_on_termination = true
  }

  user_data_replace_on_change = true
  user_data                   = <<-USER_DATA
    #!/bin/bash
    set -euo pipefail
    dnf update -y
    dnf install -y docker jq
    systemctl enable --now docker
    usermod -aG docker ec2-user

    architecture="$(uname -m)"
    curl -fsSL "https://github.com/docker/compose/releases/download/v${var.docker_compose_version}/docker-compose-linux-$${architecture}" \
      -o /usr/local/lib/docker/cli-plugins/docker-compose
    chmod 0755 /usr/local/lib/docker/cli-plugins/docker-compose

    install -d -m 0750 -o ec2-user -g ec2-user /opt/bnshop
    install -d -m 0700 -o ec2-user -g ec2-user /opt/bnshop/secrets
  USER_DATA

  tags = {
    Name = "${var.project_name}-${var.environment}-app"
  }
}

# EIP mantem o endpoint estavel entre stop/start. Ele tem o mesmo custo de um
# IPv4 publico em uso; deve ser destruido junto com o ambiente para nao ficar ocioso.
resource "aws_eip" "app" {
  domain   = "vpc"
  instance = aws_instance.app.id

  tags = {
    Name = "${var.project_name}-${var.environment}-eip"
  }
}

