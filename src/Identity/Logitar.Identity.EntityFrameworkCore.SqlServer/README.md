# Logitar.Identity.EntityFrameworkCore.SqlServer

Provides an implementation of a relational data store to be used with the Identity API, Entity
Framework Core and Microsoft SQL Server.

## Migrations

This project is setup to use migrations. You must execute the following commands in the solution
directory.

### Create a new migration

Execute the following command to create a new migration. Do not forget to specify a migration name!

`dotnet ef migrations add <YOUR_MIGRATION_NAME> --context IdentityContext --project src/Identity/Logitar.Identity.EntityFrameworkCore.SqlServer --startup-project src/Demo/Logitar.Demo.Ui`

### Generate a script

Execute the following command to generate a new script. Do not forget to specify a source migration name!

`dotnet ef migrations script <FROM_MIGRATION_NAME> --context IdentityContext --project src/Identity/Logitar.Identity.EntityFrameworkCore.SqlServer --startup-project src/Demo/Logitar.Demo.Ui`
