# Logitar.Identity.EntityFrameworkCore.SqlServer

TODO

## Migrations

This project is setup to use migrations. The commands below must be executed in the solution root directory.

### Create a new migration

To create a new migration, execute the following command. Do not forget to specify a migration name!

`dotnet ef migrations add CreateUserTable --startup-project src/Demo/Logitar.Demo.Ui --project src/Identity/Logitar.Identity.EntityFrameworkCore.SqlServer --context IdentityContext`
