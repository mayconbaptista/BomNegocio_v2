# Free tier do CloudWatch: 10 alarmes de metricas padrao. Aqui sao 6.
# Metricas basicas (5 min) de EC2/RDS sao gratuitas; detailed monitoring nao.

locals {
  name = "${var.project_name}-${var.environment}"
}

resource "aws_sns_topic" "alerts" {
  name = "${local.name}-alerts"
}

# A assinatura por e-mail fica "PendingConfirmation" ate voce clicar no link.
resource "aws_sns_topic_subscription" "email" {
  topic_arn = aws_sns_topic.alerts.arn
  protocol  = "email"
  endpoint  = var.alert_email
}

# --- EC2 -------------------------------------------------------------------

# Falha no hardware/host da AWS: a acao "recover" migra a instancia para outro
# host mantendo ID, IP privado, EIP e EBS. Self-healing sem custo.
resource "aws_cloudwatch_metric_alarm" "ec2_system_check" {
  alarm_name          = "${local.name}-ec2-system-check"
  namespace           = "AWS/EC2"
  metric_name         = "StatusCheckFailed_System"
  statistic           = "Maximum"
  period              = 60
  evaluation_periods  = 2
  threshold           = 1
  comparison_operator = "GreaterThanOrEqualToThreshold"
  dimensions          = { InstanceId = var.instance_id }
  alarm_actions       = ["arn:aws:automate:${var.aws_region}:ec2:recover", aws_sns_topic.alerts.arn]
}

# Falha no SO (kernel travado, OOM severo): reboot automatico.
resource "aws_cloudwatch_metric_alarm" "ec2_instance_check" {
  alarm_name          = "${local.name}-ec2-instance-check"
  namespace           = "AWS/EC2"
  metric_name         = "StatusCheckFailed_Instance"
  statistic           = "Maximum"
  period              = 60
  evaluation_periods  = 3
  threshold           = 1
  comparison_operator = "GreaterThanOrEqualToThreshold"
  dimensions          = { InstanceId = var.instance_id }
  alarm_actions       = ["arn:aws:automate:${var.aws_region}:ec2:reboot", aws_sns_topic.alerts.arn]
}

# Em modo "standard", creditos zerados = CPU travada no baseline (20%).
# E o sinal de que o ambiente esta subdimensionado ou com loop quente.
resource "aws_cloudwatch_metric_alarm" "ec2_cpu_credits" {
  alarm_name          = "${local.name}-ec2-cpu-credits-low"
  namespace           = "AWS/EC2"
  metric_name         = "CPUCreditBalance"
  statistic           = "Minimum"
  period              = 300
  evaluation_periods  = 3
  threshold           = 20
  comparison_operator = "LessThanThreshold"
  dimensions          = { InstanceId = var.instance_id }
  alarm_actions       = [aws_sns_topic.alerts.arn]
}

# --- RDS -------------------------------------------------------------------

resource "aws_cloudwatch_metric_alarm" "rds_free_storage" {
  alarm_name          = "${local.name}-rds-free-storage"
  namespace           = "AWS/RDS"
  metric_name         = "FreeStorageSpace"
  statistic           = "Minimum"
  period              = 300
  evaluation_periods  = 2
  threshold           = 2 * 1024 * 1024 * 1024 # 2 GiB
  comparison_operator = "LessThanThreshold"
  dimensions          = { DBInstanceIdentifier = var.db_identifier }
  alarm_actions       = [aws_sns_topic.alerts.arn]
}

# RDS T4g e SEMPRE "unlimited" (nao da para mudar). Qualquer credito excedente
# cobrado vira custo real; este alarme e o guarda-chuva.
resource "aws_cloudwatch_metric_alarm" "rds_surplus_credits" {
  alarm_name          = "${local.name}-rds-surplus-credits-charged"
  namespace           = "AWS/RDS"
  metric_name         = "CPUSurplusCreditsCharged"
  statistic           = "Maximum"
  period              = 300
  evaluation_periods  = 1
  threshold           = 0
  comparison_operator = "GreaterThanThreshold"
  treat_missing_data  = "notBreaching"
  dimensions          = { DBInstanceIdentifier = var.db_identifier }
  alarm_actions       = [aws_sns_topic.alerts.arn]
}

resource "aws_cloudwatch_metric_alarm" "rds_cpu" {
  alarm_name          = "${local.name}-rds-cpu-high"
  namespace           = "AWS/RDS"
  metric_name         = "CPUUtilization"
  statistic           = "Average"
  period              = 300
  evaluation_periods  = 6 # 30 min sustentado
  threshold           = 60
  comparison_operator = "GreaterThanThreshold"
  dimensions          = { DBInstanceIdentifier = var.db_identifier }
  alarm_actions       = [aws_sns_topic.alerts.arn]
}
