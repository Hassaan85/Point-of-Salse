#!/usr/bin/env bash
set -euo pipefail

# Wait for SQL Server to be available
/opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P "$SA_PASSWORD" -Q "WAITFOR DELAY '00:00:05'"

/opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P "$SA_PASSWORD" -Q "IF DB_ID('PosDb') IS NULL CREATE DATABASE PosDb;"

# Run schema and procedure scripts if present
if [ -d "/docker-entrypoint-initdb.d/scripts" ]; then
  for file in /docker-entrypoint-initdb.d/scripts/*.sql; do
    [ -e "$file" ] || continue
    echo "Running $file"
    /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P "$SA_PASSWORD" -d PosDb -i "$file"
  done
fi
