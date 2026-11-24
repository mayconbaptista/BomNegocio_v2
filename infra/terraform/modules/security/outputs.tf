output "security_group_id" {
  description = "Security Group a ser anexado na EC2"
  value       = aws_security_group.ec2.id
}

output "iam_instance_profile_name" {
  description = "Instance profile a ser anexado na EC2"
  value       = aws_iam_instance_profile.ec2.name
}

output "iam_role_name" {
  description = "Nome da IAM Role anexada a EC2"
  value       = aws_iam_role.ec2.name
}

output "rds_security_group_id" {
  description = "Security Group privado do RDS"
  value       = aws_security_group.rds.id
}
