output "fqdn" { value = local.fqdn }

# Vem do recurso de validacao (e nao do certificado) para que a distribuicao so
# receba o ARN depois que o ACM emitir o certificado.
output "certificate_arn" { value = aws_acm_certificate_validation.app.certificate_arn }
