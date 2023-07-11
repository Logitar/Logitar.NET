# Logitar.Demo.Ui

Demo User Interface for the Logitar.NET project.

## PostgreSQL

Execute the following command to create a PostgreSQL Docker image.

`docker run --name Logitar.Demo.Ui_postgres -e POSTGRES_PASSWORD=P@s$W0rD -p 2443:5432 -d postgres`

## SQL Server

Execute the following command to create a SQL Server Docker image.

`docker run --name Logitar.Demo.Ui_sqlserver -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=P@s$W0rD" -p 6911:1433 -d mcr.microsoft.com/mssql/server:2022-latest`
