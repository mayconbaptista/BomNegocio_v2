locals {
  ami_parameter = var.architecture == "arm64" ? "/aws/service/ami-amazon-linux-latest/al2023-ami-kernel-default-arm64" : "/aws/service/ami-amazon-linux-latest/al2023-ami-kernel-default-x86_64"
}

data "aws_ssm_parameter" "amazon_linux_2023" {
  name = local.ami_parameter
}

resource "aws_instance" "app" {
  ami                    = data.aws_ssm_parameter.amazon_linux_2023.value
  instance_type          = var.instance_type
  subnet_id              = var.subnet_id
  vpc_security_group_ids = [var.security_group_id]
  iam_instance_profile   = var.iam_instance_profile_name
  # IPv4 publico efemero: necessario para a SAIDA (sem NAT Gateway, que custa
  # ~US$ 32/mes). Cobrado so com a instancia ligada e trocado a cada boot,
  # o que nao importa: nada se conecta a ele (ver ADR-012).
  associate_public_ip_address = true
  monitoring                  = false

  # T4g nasce em modo "unlimited": acima do baseline (20% no t4g.small) a AWS
  # cobra creditos excedentes. Em "standard" a CPU e limitada ao baseline
  # quando os creditos acabam: fica lento, mas nunca gera cobranca extra.
  credit_specification {
    cpu_credits = "standard"
  }

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

    # Swap de 2 GiB: 5 runtimes .NET + Traefik em 2 GiB de RAM nao deixam
    # folga para picos (JIT, GC, pull de imagens). Swap evita OOM-kill ao custo
    # de latencia quando usado; os mem_limit do compose continuam valendo.
    if [ ! -f /swapfile ]; then
      dd if=/dev/zero of=/swapfile bs=1M count=${var.swap_size_mb}
      chmod 600 /swapfile
      mkswap /swapfile
      echo '/swapfile none swap sw 0 0' >> /etc/fstab
    fi
    swapon -a
    sysctl -w vm.swappiness=10
    echo 'vm.swappiness=10' > /etc/sysctl.d/99-bnshop.conf

    # O diretorio de plugins nao existe no AL2023; sem ele o curl falha e o
    # set -e aborta todo o bootstrap.
    install -d -m 0755 /usr/local/lib/docker/cli-plugins
    architecture="$(uname -m)"
    curl -fsSL "https://github.com/docker/compose/releases/download/v${var.docker_compose_version}/docker-compose-linux-$${architecture}" \
      -o /usr/local/lib/docker/cli-plugins/docker-compose
    chmod 0755 /usr/local/lib/docker/cli-plugins/docker-compose

    install -d -m 0750 -o root -g root /opt/bnshop
    install -d -m 0700 -o root -g root /opt/bnshop/secrets
  USER_DATA

  tags = {
    Name = "${var.project_name}-${var.environment}-app"
  }
}

