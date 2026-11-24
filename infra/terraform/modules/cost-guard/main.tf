# Disjuntor de creditos (ADR-013). O Budget sozinho so manda e-mail; aqui ele
# AGE quando o gasto bruto do mes passa do limite:
#
#   1. Para a EC2 e o RDS (SSM, via Budget Actions).
#   2. Anexa uma politica que NEGA StartInstances/StartDBInstance a role do
#      Scheduler e ao usuario do GitHub Actions. Sem isso, a agenda das 8h
#      religaria tudo no dia seguinte.
#
# No inicio do mes seguinte a AWS desfaz a acao de IAM sozinha (acoes de IAM e
# SCP resetam a cada periodo do budget) e a agenda volta a ligar o ambiente.
# As acoes de "stop" nao precisam de reset: e a agenda que liga de novo.
#
# Por que no mes e nao no total: US$ 16/mes x 6 meses = US$ 96, dentro dos
# US$ 100 de credito. Limitar cada mes garante que o total nunca estoure.

locals {
  name = "${var.project_name}-${var.environment}"
}

data "aws_caller_identity" "current" {}

# --- Politica aplicada pelo disjuntor -----------------------------------------
resource "aws_iam_policy" "deny_power_on" {
  name        = "${local.name}-budget-deny-power-on"
  description = "Aplicada pelo AWS Budgets quando o limite mensal estoura"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Sid      = "CreditosDoMesEsgotados"
      Effect   = "Deny"
      Action   = ["ec2:StartInstances", "rds:StartDBInstance"]
      Resource = "*"
    }]
  })
}

# --- Role que o AWS Budgets assume para executar as acoes ---------------------
data "aws_iam_policy_document" "budgets_trust" {
  statement {
    actions = ["sts:AssumeRole"]

    principals {
      type        = "Service"
      identifiers = ["budgets.amazonaws.com"]
    }

    condition {
      test     = "StringEquals"
      variable = "aws:SourceAccount"
      values   = [data.aws_caller_identity.current.account_id]
    }
  }
}

resource "aws_iam_role" "budget_actions" {
  name               = "${local.name}-budget-actions"
  assume_role_policy = data.aws_iam_policy_document.budgets_trust.json
}

# Politica gerenciada pela AWS feita para este caso: roda os documentos SSM
# que param instancias EC2/RDS a partir de um Budget Action.
data "aws_iam_policy" "budgets_ssm" {
  name = "AWSBudgetsActions_RolePolicyForResourceAdministrationWithSSM"
}

resource "aws_iam_role_policy_attachment" "budgets_ssm" {
  role       = aws_iam_role.budget_actions.name
  policy_arn = data.aws_iam_policy.budgets_ssm.arn
}

# Anexar/desanexar SOMENTE a politica de bloqueio, SOMENTE nos alvos.
# Usuarios ficam em path /ci/ (modules/cicd), por isso o curinga no ARN.
data "aws_iam_policy_document" "attach_deny" {
  statement {
    sid       = "Roles"
    actions   = ["iam:AttachRolePolicy", "iam:DetachRolePolicy"]
    resources = [for role in var.roles_to_block : "arn:aws:iam::${data.aws_caller_identity.current.account_id}:role/${role}"]

    condition {
      test     = "ArnEquals"
      variable = "iam:PolicyARN"
      values   = [aws_iam_policy.deny_power_on.arn]
    }
  }

  dynamic "statement" {
    for_each = length(var.users_to_block) > 0 ? [1] : []

    content {
      sid       = "Users"
      actions   = ["iam:AttachUserPolicy", "iam:DetachUserPolicy"]
      resources = [for user in var.users_to_block : "arn:aws:iam::${data.aws_caller_identity.current.account_id}:user/*/${user}"]

      condition {
        test     = "ArnEquals"
        variable = "iam:PolicyARN"
        values   = [aws_iam_policy.deny_power_on.arn]
      }
    }
  }
}

resource "aws_iam_role_policy" "attach_deny" {
  name   = "attach-deny-power-on"
  role   = aws_iam_role.budget_actions.id
  policy = data.aws_iam_policy_document.attach_deny.json
}

# --- Acoes do budget ------------------------------------------------------------
locals {
  stop_actions = {
    ec2 = { sub_type = "STOP_EC2_INSTANCES", ids = [var.instance_id] }
    rds = { sub_type = "STOP_RDS_INSTANCES", ids = [var.db_instance_identifier] }
  }
}

resource "aws_budgets_budget_action" "stop" {
  for_each = local.stop_actions

  budget_name        = var.budget_name
  action_type        = "RUN_SSM_DOCUMENTS"
  approval_model     = "AUTOMATIC" # sem esperar alguem clicar em "aprovar"
  notification_type  = "ACTUAL"
  execution_role_arn = aws_iam_role.budget_actions.arn

  action_threshold {
    action_threshold_type  = "PERCENTAGE"
    action_threshold_value = var.threshold_percent
  }

  definition {
    ssm_action_definition {
      action_sub_type = each.value.sub_type
      instance_ids    = each.value.ids
      region          = var.aws_region
    }
  }

  subscriber {
    address           = var.alert_email
    subscription_type = "EMAIL"
  }

  depends_on = [aws_iam_role_policy_attachment.budgets_ssm]
}

resource "aws_budgets_budget_action" "block_power_on" {
  budget_name        = var.budget_name
  action_type        = "APPLY_IAM_POLICY"
  approval_model     = "AUTOMATIC"
  notification_type  = "ACTUAL"
  execution_role_arn = aws_iam_role.budget_actions.arn

  action_threshold {
    action_threshold_type  = "PERCENTAGE"
    action_threshold_value = var.threshold_percent
  }

  definition {
    iam_action_definition {
      policy_arn = aws_iam_policy.deny_power_on.arn
      roles      = var.roles_to_block
      users      = length(var.users_to_block) > 0 ? var.users_to_block : null
    }
  }

  subscriber {
    address           = var.alert_email
    subscription_type = "EMAIL"
  }

  depends_on = [aws_iam_role_policy.attach_deny]
}
