# Logitar.Identity.EntityFrameworkCore.PostgreSQL

Provides an implementation of a relational identity store to be used with Entity Framework Core and
PostgreSQL.

## Migrations

This project is setup to use migrations. You must execute the following commands in the solution
directory.

### Create a new migration

Execute the following command to create a new migration. Do not forget to specify a migration name!

`dotnet ef migrations add <YOUR_MIGRATION_NAME> --startup-project src/Demo/Logitar.Demo.Ui --project src/Identity/Logitar.Identity.EntityFrameworkCore.PostgreSQL --context IdentityContext`
