#!/bin/bash

echo "waiting for Postgresql start..."

sleep 10

psql -U "$POSTGRES_USER" -d "$POSTGRES_DB" -f /docker-entrypoint-initdb.d/init.sql

# set -e


#psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
#	CREATE USER docker;
#    CREATE USER catalog_user;
#	CREATE DATABASE docker;
#	GRANT ALL PRIVILEGES ON DATABASE docker TO docker;
#EOSQL
