# Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL.IntegrationTests

This project provider integration tests for the Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL
project. In order to run the integration tests, you must have a running PostgreSQL database. The
easiest way to do so is by using Docker and running the following command, using the default values.
The connection string is already configured in the `appsettings.json` file.

`docker run --name Logitar.Demo.Ui_postgres -e POSTGRES_PASSWORD=P@s$W0rD -p 2443:5432 -d postgres`
