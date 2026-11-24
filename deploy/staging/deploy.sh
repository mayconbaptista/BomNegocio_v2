#!/usr/bin/env bash
# Deploy idempotente do BnEshop staging. Executado NA EC2, como root, pelo
# SSM Run Command disparado pelo GitHub Actions:
#
#   aws s3 cp --recursive s3://<private>/deploy/<sha>/ /opt/bnshop/releases/<sha>/
#   bash /opt/bnshop/releases/<sha>/deploy.sh <sha>
#
# Rollback = executar de novo com o sha anterior (cat /opt/bnshop/previous).
# Migrations NAO sao revertidas: use expand/contract para manter o schema
# compativel com a versao anterior.
set -euo pipefail

SHA="${1:?uso: deploy.sh <git-sha>}"
PROJECT="bnshop"
ENVIRONMENT="staging"
PREFIX="/${PROJECT}/${ENVIRONMENT}"
BASE_DIR="/opt/bnshop"
RELEASE_DIR="${BASE_DIR}/releases/${SHA}"
RUN_DIR="/run/bnshop" # tmpfs: segredos ficam so em memoria
ENV_FILE="${RUN_DIR}/staging.env"
EFI_CERT="${BASE_DIR}/secrets/efi-certificate.p12"
COMPOSE=(docker compose --project-name bneshop-staging
  --file "${RELEASE_DIR}/docker-compose.staging.yml"
  --env-file "${ENV_FILE}")

log() { printf '[deploy %s] %s\n' "$(date -u +%H:%M:%S)" "$*"; }

cleanup() { rm -f "${ENV_FILE}"; }
trap cleanup EXIT

# --- Regiao via IMDSv2 (a EC2 exige token: http_tokens = required) ----------
IMDS_TOKEN="$(curl -fsS -X PUT http://169.254.169.254/latest/api/token \
  -H 'X-aws-ec2-metadata-token-ttl-seconds: 300')"
AWS_REGION="$(curl -fsS -H "X-aws-ec2-metadata-token: ${IMDS_TOKEN}" \
  http://169.254.169.254/latest/meta-data/placement/region)"
export AWS_REGION AWS_DEFAULT_REGION="${AWS_REGION}"

# --- Parametros (GetParameters em lote; a role nao tem GetParametersByPath) --
declare -A P
fetch_parameters() {
  local names=()
  for n in "$@"; do names+=("${PREFIX}/${n}"); done

  local json
  json="$(aws ssm get-parameters --with-decryption --names "${names[@]}" --output json)"

  local invalid
  invalid="$(jq -r '.InvalidParameters[]?' <<<"${json}")"
  if [[ -n "${invalid}" ]]; then
    # efi e opcional: so e necessario quando a integracao Pix estiver ativa.
    invalid="$(grep -v 'efi-certificate-password' <<<"${invalid}" || true)"
    if [[ -n "${invalid}" ]]; then
      log "Parametros ausentes no SSM:"; echo "${invalid}"
      log "Rode infra/scripts/Set-StagingSecrets.ps1 e tente novamente."
      exit 1
    fi
  fi

  while IFS=$'\t' read -r name value; do
    P["${name#"${PREFIX}/"}"]="${value}"
  done < <(jq -r '.Parameters[] | [.Name, .Value] | @tsv' <<<"${json}")
}

log "Lendo Parameter Store (${PREFIX})"
fetch_parameters config/catalog-bucket config/private-bucket config/rds-endpoint \
  config/ecr-registry config/auth-valid-audience config/auth-valid-issuer
# GetParameters aceita no maximo 10 nomes por chamada.
fetch_parameters secrets/origin-verify-secret secrets/cloudflare-tunnel-token secrets/auth-signing-key \
  secrets/auth-db-password secrets/cart-db-password secrets/catalog-db-password \
  secrets/order-db-password secrets/efi-certificate-password

REGISTRY="${P[config/ecr-registry]}"
RDS="${P[config/rds-endpoint]}"

conn() { # conn <database> <user> <password>
  printf 'Host=%s;Port=5432;Database=%s;Username=%s;Password=%s;SSL Mode=Require' \
    "${RDS}" "$1" "$2" "$3"
}

# --- Certificado Efi (opcional) ------------------------------------------------
install -d -m 0700 "${BASE_DIR}/secrets"
if aws s3 cp "s3://${P[config/private-bucket]}/efi/efi-certificate.p12" "${EFI_CERT}.tmp" --only-show-errors 2>/dev/null; then
  mv "${EFI_CERT}.tmp" "${EFI_CERT}"
  chmod 0644 "${EFI_CERT}" # lido pelo usuario nao-root do container (APP_UID)
else
  log "AVISO: certificado Efi nao encontrado no S3; Pix ficara indisponivel."
  [[ -f "${EFI_CERT}" ]] || install -m 0644 /dev/null "${EFI_CERT}"
fi

# --- Gera o env file (somente em tmpfs) -------------------------------------
install -d -m 0700 "${RUN_DIR}"
umask 077
cat >"${ENV_FILE}" <<EOF
AWS_REGION=${AWS_REGION}
ORIGIN_VERIFY_SECRET=${P[secrets/origin-verify-secret]}
CLOUDFLARE_TUNNEL_TOKEN=${P[secrets/cloudflare-tunnel-token]}
GATEWAY_IMAGE=${REGISTRY}/${PROJECT}/gateway:${SHA}
AUTH_IMAGE=${REGISTRY}/${PROJECT}/auth:${SHA}
CART_IMAGE=${REGISTRY}/${PROJECT}/cart:${SHA}
CATALOG_IMAGE=${REGISTRY}/${PROJECT}/catalog:${SHA}
ORDER_IMAGE=${REGISTRY}/${PROJECT}/order:${SHA}
AUTH_VALID_AUDIENCE=${P[config/auth-valid-audience]}
AUTH_VALID_ISSUER=${P[config/auth-valid-issuer]}
AUTH_SIGNING_KEY=${P[secrets/auth-signing-key]}
AUTH_DB_CONNECTION=$(conn auth auth_app "${P[secrets/auth-db-password]}")
CART_DB_CONNECTION=$(conn cart cart_app "${P[secrets/cart-db-password]}")
CATALOG_DB_CONNECTION=$(conn catalog catalog_app "${P[secrets/catalog-db-password]}")
ORDER_DB_CONNECTION=$(conn orders order_app "${P[secrets/order-db-password]}")
CATALOG_BUCKET=${P[config/catalog-bucket]}
EFI_CERTIFICATE_HOST_PATH=${EFI_CERT}
EFI_CERTIFICATE_PASSWORD=${P[secrets/efi-certificate-password]:-}
EOF

# --- Pull das imagens ---------------------------------------------------------
log "Login no ECR ${REGISTRY}"
aws ecr get-login-password | docker login --username AWS --password-stdin "${REGISTRY}" >/dev/null

log "Pull das imagens ${SHA}"
"${COMPOSE[@]}" pull --quiet

# --- Migrations: job unico, sequencial, ANTES de trocar os containers --------
# Nunca no startup de cada replica: com N replicas haveria N migrations
# concorrentes disputando o lock da tabela __EFMigrationsHistory.
for svc in auth.webapi cart.webapi catalog.api order.webapi; do
  log "Migrations: ${svc}"
  "${COMPOSE[@]}" run --rm --no-deps "${svc}" --migrate
done

# --- Sobe a nova versao -------------------------------------------------------
log "docker compose up"
"${COMPOSE[@]}" up --detach --remove-orphans

# --- Health check pelo loopback (as APIs nao sao expostas publicamente) ------
check() { # check <nome> <url>
  for _ in $(seq 1 30); do
    if curl -fsS -o /dev/null "$2"; then log "OK ${1}"; return 0; fi
    sleep 4
  done
  log "FALHA ${1} (${2})"; return 1
}

healthy=true
check gateway "http://127.0.0.1:18080/healthz" || healthy=false
check auth    "http://127.0.0.1:18235/swagger/v1/swagger.json" || healthy=false
check cart    "http://127.0.0.1:18087/swagger/v1/swagger.json" || healthy=false
check catalog "http://127.0.0.1:18989/swagger/v1/swagger.json" || healthy=false
check order   "http://127.0.0.1:18888/swagger/v1/swagger.json" || healthy=false

# Tunnel: sem conexao registrada na Cloudflare, o site responde 530 mesmo com
# as APIs saudaveis. O cloudflared abre 4 conexoes; uma ja basta para servir.
tunnel_connected=false
for _ in $(seq 1 30); do
  if "${COMPOSE[@]}" logs cloudflared 2>&1 | grep -q "Registered tunnel connection"; then
    tunnel_connected=true; break
  fi
  sleep 2
done
if [[ "${tunnel_connected}" == true ]]; then log "OK tunnel"; else log "FALHA tunnel (cloudflared sem conexao)"; healthy=false; fi

if [[ "${healthy}" != true ]]; then
  "${COMPOSE[@]}" ps
  "${COMPOSE[@]}" logs --tail 60
  PREV="$(cat "${BASE_DIR}/current" 2>/dev/null || true)"
  log "Deploy ${SHA} nao ficou saudavel. Rollback: deploy.sh ${PREV:-<sha-anterior>}"
  exit 1
fi

# --- Registra a release e limpa imagens antigas (EBS de 16 GB) ---------------
if [[ -f "${BASE_DIR}/current" ]]; then cp "${BASE_DIR}/current" "${BASE_DIR}/previous"; fi
echo "${SHA}" >"${BASE_DIR}/current"
docker image prune --all --force --filter "until=24h" >/dev/null
find "${BASE_DIR}/releases" -mindepth 1 -maxdepth 1 -type d -mtime +7 -exec rm -rf {} +

log "Deploy ${SHA} concluido"
