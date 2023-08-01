using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Core.Models;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.SqlServer;
using Logitar.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.IntegrationTests;

public abstract class IntegrationTestingBase : IAsyncLifetime
{
  static IntegrationTestingBase()
  {
    IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json")
      .Build();

    var services = new ServiceCollection()
      .AddSingleton(configuration)
      .Configure<Pbkdf2Settings>(settings => settings.Iterations = 6);

    string connectionString;
    DatabaseProvider = configuration.GetValue<DatabaseProvider?>("DatabaseProvider")
      ?? DatabaseProvider.EntityFrameworkCoreSqlServer;
    switch (DatabaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCorePostgreSQL:
        connectionString = configuration.GetValue<string>("POSTGRESQLCONNSTR_IntegrationTests") ?? string.Empty;
        services = services.AddLogitarIdentityWithEntityFrameworkCorePostgreSQL(connectionString);
        break;
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        connectionString = configuration.GetValue<string>("SQLCONNSTR_IntegrationTests") ?? string.Empty;
        services = services.AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString);
        break;
      default:
        throw new NotSupportedException($"The database provider '{DatabaseProvider}' is not supported.");
    }

    ServiceProvider = services.BuildServiceProvider();
  }

  protected IntegrationTestingBase()
  {
    EventContext = ServiceProvider.GetRequiredService<EventContext>();
    IdentityContext = ServiceProvider.GetRequiredService<IdentityContext>();
  }

  protected static readonly Actor Actor = new();
  protected static readonly CancellationToken CancellationToken = default;
  protected static readonly DatabaseProvider DatabaseProvider;
  protected static readonly IServiceProvider ServiceProvider;

  protected readonly Bogus.Faker Faker = new();

  protected readonly EventContext EventContext;
  protected readonly IdentityContext IdentityContext;

  public virtual async Task InitializeAsync()
  {
    await EventContext.Database.MigrateAsync();
    switch (DatabaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCorePostgreSQL:
        await EventContext.Database.ExecuteSqlRawAsync(@"DELETE FROM ""public"".""Events"";");
        break;
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        await EventContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Events];");
        break;
    }

    await IdentityContext.Database.MigrateAsync();
    switch (DatabaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCorePostgreSQL:
        await IdentityContext.Database.ExecuteSqlRawAsync(@"DELETE FROM ""public"".""ApiKeys"";");
        await IdentityContext.Database.ExecuteSqlRawAsync(@"DELETE FROM ""public"".""ExternalIdentifiers"";");
        await IdentityContext.Database.ExecuteSqlRawAsync(@"DELETE FROM ""public"".""Roles"";");
        await IdentityContext.Database.ExecuteSqlRawAsync(@"DELETE FROM ""public"".""Sessions"";");
        await IdentityContext.Database.ExecuteSqlRawAsync(@"DELETE FROM ""public"".""TokenBlacklist"";");
        await IdentityContext.Database.ExecuteSqlRawAsync(@"DELETE FROM ""public"".""Users"";");
        break;
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[ApiKeys];");
        await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[ExternalIdentifiers];");
        await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Roles];");
        await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Sessions];");
        await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[TokenBlacklist];");
        await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Users];");
        break;
    }
  }

  public Task DisposeAsync() => Task.CompletedTask;
}
