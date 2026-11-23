#!/usr/bin/env bash
# Cria um database e um usuario por microsservico no RDS. Executar UMA vez
# (e idempotente, pode repetir) via SSM Session Manager na EC2, que e o unico
# host com acesso de rede ao RDS:
#
#   aws ssm start-session --target <instance-id>
#   sudo bash /opt/bnshop/releases/<sha>/bootstrap-db.sh
#
# Database-per-service em uma instancia fisica: isolamento logico (cada
# usuario so conecta no proprio database) sem pagar 4 instancias RDS.
set -euo pipefail

PREFIX="/bnshop/staging"
IMDS_TOKEN="$(curl -fsS -X PUT http://169.254.169.254/latest/api/token \
  -H 'X-aws-ec2-metadata-token-ttl-seconds: 300')"
export AWS_DEFAULT_REGION="$(curl -fsS -H "X-aws-ec2-metadata-token: ${IMDS_TOKEN}" \
  http://169.254.169.254/latest/meta-data/placement/region)"

param() { aws ssm get-parameter --with-decryption --name "${PREFIX}/$1" --query Parameter.Value --output text; }

RDS="$(param config/rds-endpoint)"
MASTER_USER="${MASTER_USER:-bnadmin}"

# A senha master nunca e persistida (nem no SSM, nem no state): digite aqui.
read -r -s -p "Senha master do RDS (${MASTER_USER}): " MASTER_PASSWORD; echo

# SQL via stdin: senhas nao aparecem na linha de comando (ps aux).
{
  echo "\\set ON_ERROR_STOP on"
  for pair in auth:auth cart:cart catalog:catalog orders:order; do
    db="${pair%%:*}"; svc="${pair##*:}"; role="${svc}_app"
    pw="$(param "secrets/${svc}-db-password")"
    cat <<SQL
\\set pw '${pw}'
SELECT format('CREATE ROLE %I LOGIN', '${role}')
 WHERE NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = '${role}') \\gexec
ALTER ROLE ${role} WITH LOGIN PASSWORD :'pw';
-- PG16: o criador precisa poder "SET ROLE" para atribuir ownership.
GRANT ${role} TO CURRENT_USER;
SELECT format('CREATE DATABASE %I OWNER %I', '${db}', '${role}')
 WHERE NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = '${db}') \\gexec
REVOKE ALL ON DATABASE ${db} FROM PUBLIC;
GRANT CONNECT, TEMPORARY ON DATABASE ${db} TO ${role};
SQL
  done
} | docker run --rm -i \
      -e PGPASSWORD="${MASTER_PASSWORD}" -e PGSSLMODE=require \
      postgres:16-alpine \
      psql --host "${RDS}" --username "${MASTER_USER}" --dbname postgres --quiet

echo "Databases e usuarios prontos. Teste de isolamento:"
echo "  cart_app NAO deve conseguir conectar em 'auth' (permission denied)."
