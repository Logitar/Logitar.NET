# Logitar.EventSourcing.EntityFrameworkCore.SqlServer

Provides an implementation of a relational event store to be used with the Event Sourcing
architecture pattern, Entity Framework Core and Microsoft SQL Server.

## Migrations

This project is setup to use migrations. You must execute the following commands in the solution
directory.

### Create a new migration

Execute the following command to create a new migration. Do not forget to specify a migration name!

`dotnet ef migrations add <YOUR_MIGRATION_NAME> --project src/Logitar.EventSourcing.EntityFrameworkCore.SqlServer --startup-project src/Logitar.Demo.Ui`
