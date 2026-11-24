# Um repositorio por microservico: ciclo de vida, permissao e lifecycle
# independentes (cada servico versiona e faz rollback sozinho).
resource "aws_ecr_repository" "service" {
  for_each = toset(var.services)

  name                 = "${var.project_name}/${each.key}"
  image_tag_mutability = "IMMUTABLE" # tag = git sha; nunca sobrescrever
  force_delete         = var.force_delete

  image_scanning_configuration {
    scan_on_push = true # basic scanning e gratuito
  }

  encryption_configuration {
    encryption_type = "AES256" # KMS customer-managed custaria US$ 1/mes
  }
}

# ECR privado: 500 MB/mes gratuitos no primeiro ano. Uma imagem ASP.NET arm64
# comprimida tem ~90-120 MB; 5 servicos x 2 versoes ja encosta no limite.
# Manter poucas versoes e o principal controle de custo aqui.
resource "aws_ecr_lifecycle_policy" "service" {
  for_each   = aws_ecr_repository.service
  repository = each.value.name

  policy = jsonencode({
    rules = [
      {
        rulePriority = 1
        description  = "Remove imagens sem tag apos 1 dia"
        selection = {
          tagStatus   = "untagged"
          countType   = "sinceImagePushed"
          countUnit   = "days"
          countNumber = 1
        }
        action = { type = "expire" }
      },
      {
        rulePriority = 2
        description  = "Mantem somente as N imagens mais recentes (atual + rollback)"
        selection = {
          tagStatus   = "any"
          countType   = "imageCountMoreThan"
          countNumber = var.images_to_keep
        }
        action = { type = "expire" }
      }
    ]
  })
}
