using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Core.Models;
using Logitar.Identity.EntityFrameworkCore.SqlServer;
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

    string connectionString = configuration.GetValue<string>("SQLCONNSTR_IntegrationTests") ?? string.Empty;

    ServiceProvider = new ServiceCollection()
      .AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString)
      .AddSingleton(configuration)
      .AddSingleton<ICurrentActor>(new CurrentActorMock(Actor))
      .BuildServiceProvider();
  }

  protected IntegrationTestingBase()
  {
    EventContext = ServiceProvider.GetRequiredService<EventContext>();
    IdentityContext = ServiceProvider.GetRequiredService<IdentityContext>();
  }

  protected static readonly Actor Actor = new();
  protected static readonly CancellationToken CancellationToken = default;
  protected static readonly IServiceProvider ServiceProvider;

  protected readonly Bogus.Faker Faker = new();

  protected readonly EventContext EventContext;
  protected readonly IdentityContext IdentityContext;

  public virtual async Task InitializeAsync()
  {
    await EventContext.Database.MigrateAsync();
    await EventContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Events];");

    await IdentityContext.Database.MigrateAsync();
    await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[ApiKeys];");
    await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Roles];");
    await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Sessions];");
    await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Tokenblacklist];");
    await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Users];");
  }

  public Task DisposeAsync() => Task.CompletedTask;
}
