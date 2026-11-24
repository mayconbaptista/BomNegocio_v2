# Conta nova (Free Plan) paga tudo com creditos. Por padrao o Budget usa custo
# LIQUIDO (include_credit = true): creditos abatem o gasto, o valor fica ~US$ 0
# e nenhum alerta dispara. Excluindo creditos e reembolsos, o budget passa a
# medir o consumo BRUTO, ou seja, a velocidade com que os creditos acabam.
resource "aws_budgets_budget" "monthly" {
  name         = "${var.project_name}-${var.environment}-monthly"
  budget_type  = "COST"
  limit_amount = tostring(var.limit_usd)
  limit_unit   = "USD"
  time_unit    = "MONTHLY"

  cost_types {
    include_credit = false
    include_refund = false
  }

  # O custo fixo (~US$ 4/mes) ja consome parte do limite desde o dia 1, entao
  # previsao em 50% dispararia todo mes. Alertas so quando o mes sai do plano.
  notification {
    comparison_operator        = "GREATER_THAN"
    threshold                  = 100
    threshold_type             = "PERCENTAGE"
    notification_type          = "FORECASTED"
    subscriber_email_addresses = [var.email]
  }

  notification {
    comparison_operator        = "GREATER_THAN"
    threshold                  = 90
    threshold_type             = "PERCENTAGE"
    notification_type          = "ACTUAL"
    subscriber_email_addresses = [var.email]
  }

  notification {
    comparison_operator        = "GREATER_THAN"
    threshold                  = 100
    threshold_type             = "PERCENTAGE"
    notification_type          = "ACTUAL"
    subscriber_email_addresses = [var.email]
  }
}
