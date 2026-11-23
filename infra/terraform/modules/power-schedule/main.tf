# Liga/desliga o staging por horario com EventBridge Scheduler + "universal
# targets" (chamada direta da API da AWS, sem Lambda). Free tier: 14 milhoes
# de invocacoes/mes; aqui sao ~120.
#
# Ordem importa:
#   liga:    RDS primeiro (leva 5-10 min) -> EC2 ~20 min depois
#   desliga: EC2 primeiro (app para de abrir conexoes) -> RDS em seguida
#
# Os containers voltam sozinhos no boot (restart: unless-stopped + Docker
# habilitado no systemd); o env de cada container fica gravado na config do
# Docker, entao nao e preciso rodar o deploy.sh de novo.

locals {
  name = "${var.project_name}-${var.environment}"

  # Universal target: arn:aws:scheduler:::aws-sdk:<servico>:<acao com inicial
  # minuscula>. O input usa os nomes do SDK, por isso "DbInstanceIdentifier"
  # (e nao "DBInstanceIdentifier", como na API Reference do RDS).
  actions = {
    rds-start = {
      description = "Liga o RDS antes da EC2"
      arn         = "arn:aws:scheduler:::aws-sdk:rds:startDBInstance"
      input       = { DbInstanceIdentifier = var.db_instance_identifier }
      expression  = var.schedule.rds_start
    }
    ec2-start = {
      description = "Liga a EC2 (containers sobem pelo restart policy)"
      arn         = "arn:aws:scheduler:::aws-sdk:ec2:startInstances"
      input       = { InstanceIds = [var.instance_id] }
      expression  = var.schedule.ec2_start
    }
    ec2-stop = {
      description = "Desliga a EC2"
      arn         = "arn:aws:scheduler:::aws-sdk:ec2:stopInstances"
      input       = { InstanceIds = [var.instance_id] }
      expression  = var.schedule.ec2_stop
    }
    rds-stop = {
      description = "Desliga o RDS depois da EC2"
      arn         = "arn:aws:scheduler:::aws-sdk:rds:stopDBInstance"
      input       = { DbInstanceIdentifier = var.db_instance_identifier }
      expression  = var.schedule.rds_stop
    }
  }
}

data "aws_iam_policy_document" "scheduler_trust" {
  statement {
    actions = ["sts:AssumeRole"]

    principals {
      type        = "Service"
      identifiers = ["scheduler.amazonaws.com"]
    }

    # Evita "confused deputy": so schedules desta conta assumem a role.
    condition {
      test     = "StringEquals"
      variable = "aws:SourceAccount"
      values   = [var.aws_account_id]
    }
  }
}

resource "aws_iam_role" "scheduler" {
  name               = "${local.name}-power-scheduler"
  assume_role_policy = data.aws_iam_policy_document.scheduler_trust.json
}

# Menor privilegio: somente esta instancia e este banco. Os volumes usam as
# chaves gerenciadas aws/ebs e aws/rds, cujas key policies ja autorizam o uso
# via servico; por isso nao ha permissao de KMS aqui.
data "aws_iam_policy_document" "scheduler" {
  statement {
    sid       = "PowerEc2"
    actions   = ["ec2:StartInstances", "ec2:StopInstances"]
    resources = [var.instance_arn]
  }

  statement {
    sid       = "PowerRds"
    actions   = ["rds:StartDBInstance", "rds:StopDBInstance"]
    resources = [var.db_instance_arn]
  }
}

resource "aws_iam_role_policy" "scheduler" {
  name   = "power"
  role   = aws_iam_role.scheduler.id
  policy = data.aws_iam_policy_document.scheduler.json
}

resource "aws_scheduler_schedule_group" "power" {
  name = "${local.name}-power"
}

resource "aws_scheduler_schedule" "power" {
  for_each = local.actions

  name        = "${local.name}-${each.key}"
  group_name  = aws_scheduler_schedule_group.power.name
  description = each.value.description

  # DISABLED mantem os recursos e so pausa a agenda (ex.: semana de entrevistas
  # com o ambiente 24/7). Basta mudar a variavel e aplicar.
  state                        = var.enabled ? "ENABLED" : "DISABLED"
  schedule_expression          = each.value.expression
  schedule_expression_timezone = var.timezone

  flexible_time_window {
    mode = "OFF"
  }

  target {
    arn      = each.value.arn
    role_arn = aws_iam_role.scheduler.arn
    input    = jsonencode(each.value.input)

    # Erros transitorios (throttling, banco em "backing-up") ganham novas
    # tentativas; depois de 15 min o evento e descartado, em vez das 24 h do
    # default, para nao ligar o ambiente horas fora do horario.
    retry_policy {
      maximum_event_age_in_seconds = 900
      maximum_retry_attempts       = 3
    }
  }
}
