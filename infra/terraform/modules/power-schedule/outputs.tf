output "schedule_group_name" { value = aws_scheduler_schedule_group.power.name }
output "scheduler_role_arn" { value = aws_iam_role.scheduler.arn }
output "scheduler_role_name" { value = aws_iam_role.scheduler.name }
