using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.SqlServer;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;

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

    UserSettings = ServiceProvider.GetRequiredService<IOptions<UserSettings>>();

    UserRepository = ServiceProvider.GetRequiredService<IUserRepository>();

    UserSettings userSettings = UserSettings.Value;
    User = new(userSettings.UniqueNameSettings, uniqueName: Faker.Person.UserName, tenantId: Guid.NewGuid().ToString())
    {
      Email = new(Faker.Person.Email, isVerified: true),
      FirstName = Faker.Person.FirstName,
      LastName = Faker.Person.LastName,
      Locale = CultureInfo.GetCultureInfo("en-US")
    };
  }

  protected static readonly Actor Actor = new();
  protected static readonly CancellationToken CancellationToken = default;
  protected static readonly IServiceProvider ServiceProvider;

  protected readonly Bogus.Faker Faker = new();

  protected readonly EventContext EventContext;
  protected readonly IdentityContext IdentityContext;

  protected readonly IOptions<UserSettings> UserSettings;

  protected readonly IUserRepository UserRepository;

  protected readonly Guid BlacklistedTokenId = Guid.NewGuid();
  protected readonly UserAggregate User;

  public async Task InitializeAsync()
  {
    await EventContext.Database.MigrateAsync();
    await EventContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Events];");

    await IdentityContext.Database.MigrateAsync();
    await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Users];");
    await IdentityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Tokenblacklist];");
    await UserRepository.SaveAsync(User);

    BlacklistedTokenEntity blacklisted = new(BlacklistedTokenId);
    IdentityContext.TokenBlacklist.Add(blacklisted);
    await IdentityContext.SaveChangesAsync();
  }

  public Task DisposeAsync() => Task.CompletedTask;
}
