output "security_group_id" {
  description = "Security Group a ser anexado na EC2"
  value       = aws_security_group.ec2.id
}

output "iam_instance_profile_name" {
  description = "Instance profile a ser anexado na EC2"
  value       = aws_iam_instance_profile.ec2.name
}

output "cloudflare_token_parameter_name" {
  description = "Nome do parametro SSM; o user_data da EC2 busca o valor por este nome no boot"
  value       = aws_ssm_parameter.cloudflare_api_token.name
}
