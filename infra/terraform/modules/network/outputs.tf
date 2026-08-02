output "vpc_id" {
  description = "ID da VPC criada"
  value       = aws_vpc.main.id
}

output "public_subnet_id" {
  description = "ID da subnet publica onde a EC2 sera criada"
  value       = aws_subnet.public.id
}

output "internet_gateway_id" {
  description = "ID do Internet Gateway"
  value       = aws_internet_gateway.main.id
}

output "database_subnet_ids" {
  description = "Subnets privadas que formam o DB subnet group"
  value       = aws_subnet.database[*].id
}
