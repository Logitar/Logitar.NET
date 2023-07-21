# Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL

Provides an abstraction of a relational event store to be used with the Event Sourcing architecture
pattern, Entity Framework Core and PostgreSQL.

## Migrations

This project is setup to use migrations. You must execute the following commands in the solution
directory.

### Create a new migration

Execute the following command to create a new migration.

`dotnet ef migrations add <YOUR_MIGRATION_NAME> --project src/Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL --startup-project src/Logitar.Demo.Ui`
