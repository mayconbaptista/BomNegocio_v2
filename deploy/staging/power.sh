#!/usr/bin/env bash
# Liga/desliga o staging fora da agenda, na ordem certa:
#   start: RDS -> (available) -> EC2 -> (running) -> agente SSM online
#   stop:  EC2 -> (stopped)   -> RDS
#   check: exit 0 somente se tudo estiver pronto para receber deploy
#
# Roda no GitHub Actions (workflows "Power staging" e "Deploy staging") ou na
# sua maquina (Git Bash/WSL com AWS CLI v2 e credenciais ativas):
#   EC2_INSTANCE_ID=i-... DB_INSTANCE_ID=bnshop-staging-postgres bash deploy/staging/power.sh start
#
# A agenda (EventBridge Scheduler) continua valendo: os desligamentos rodam
# todo dia as 20h e as 23h. Ligou depois das 23h? Rode "stop" ao terminar.
set -euo pipefail

action="${1:-status}"
: "${EC2_INSTANCE_ID:?Defina EC2_INSTANCE_ID}"
: "${DB_INSTANCE_ID:?Defina DB_INSTANCE_ID}"

log() { printf '[power] %s\n' "$*"; }

ec2_state() {
  aws ec2 describe-instances --instance-ids "${EC2_INSTANCE_ID}" \
    --query 'Reservations[0].Instances[0].State.Name' --output text
}

rds_state() {
  aws rds describe-db-instances --db-instance-identifier "${DB_INSTANCE_ID}" \
    --query 'DBInstances[0].DBInstanceStatus' --output text
}

ssm_ping() {
  aws ssm describe-instance-information \
    --filters "Key=InstanceIds,Values=${EC2_INSTANCE_ID}" \
    --query 'InstanceInformationList[0].PingStatus' --output text 2>/dev/null || echo "None"
}

# Espera um estado com polling proprio (nao existe waiter "db-instance-stopped").
wait_for() {
  local what="$1" expected="$2" attempts="$3" interval="$4" current
  for ((i = 1; i <= attempts; i++)); do
    current="$("${what}")"
    [[ "${current}" == "${expected}" ]] && return 0
    sleep "${interval}"
  done
  log "Timeout esperando ${what} = ${expected} (atual: ${current})"
  return 1
}

status() {
  log "EC2 ${EC2_INSTANCE_ID}: $(ec2_state) | SSM: $(ssm_ping)"
  log "RDS ${DB_INSTANCE_ID}: $(rds_state)"
}

start() {
  local db ec2
  db="$(rds_state)"
  case "${db}" in
    available) log "RDS ja esta available" ;;
    stopped)
      log "Ligando RDS (costuma levar 5-10 min)"
      aws rds start-db-instance --db-instance-identifier "${DB_INSTANCE_ID}" >/dev/null
      ;;
    stopping)
      log "RDS ainda desligando; aguardando para religar"
      wait_for rds_state stopped 60 15
      aws rds start-db-instance --db-instance-identifier "${DB_INSTANCE_ID}" >/dev/null
      ;;
    *) log "RDS em '${db}'; aguardando ficar available" ;;
  esac
  aws rds wait db-instance-available --db-instance-identifier "${DB_INSTANCE_ID}"
  log "RDS available"

  ec2="$(ec2_state)"
  case "${ec2}" in
    running | pending) ;;
    stopped)
      log "Ligando EC2"
      aws ec2 start-instances --instance-ids "${EC2_INSTANCE_ID}" >/dev/null
      ;;
    stopping)
      log "EC2 ainda desligando; aguardando para religar"
      aws ec2 wait instance-stopped --instance-ids "${EC2_INSTANCE_ID}"
      aws ec2 start-instances --instance-ids "${EC2_INSTANCE_ID}" >/dev/null
      ;;
    *)
      log "EC2 em estado inesperado: ${ec2}"
      exit 1
      ;;
  esac
  aws ec2 wait instance-running --instance-ids "${EC2_INSTANCE_ID}"

  # Sem o agente SSM online, o SendCommand do deploy falha com InvalidInstanceId.
  wait_for ssm_ping Online 60 5
  log "EC2 running e SSM online. Os containers sobem sozinhos em ~1-2 min."
}

stop() {
  case "$(ec2_state)" in
    running | pending)
      log "Desligando EC2"
      aws ec2 stop-instances --instance-ids "${EC2_INSTANCE_ID}" >/dev/null
      aws ec2 wait instance-stopped --instance-ids "${EC2_INSTANCE_ID}"
      ;;
    *) log "EC2 ja esta $(ec2_state)" ;;
  esac

  case "$(rds_state)" in
    available)
      log "Desligando RDS"
      aws rds stop-db-instance --db-instance-identifier "${DB_INSTANCE_ID}" >/dev/null
      ;;
    *) log "RDS em '$(rds_state)'; nada a fazer" ;;
  esac
  log "Desligamento solicitado"
}

check() {
  local ec2 db ping
  ec2="$(ec2_state)"
  db="$(rds_state)"
  ping="$(ssm_ping)"
  log "EC2=${ec2} SSM=${ping} RDS=${db}"
  [[ "${ec2}" == "running" && "${ping}" == "Online" && "${db}" == "available" ]]
}

case "${action}" in
  start) start ;;
  stop) stop ;;
  status) status ;;
  check) check ;;
  *)
    echo "uso: $0 start|stop|status|check" >&2
    exit 2
    ;;
esac
