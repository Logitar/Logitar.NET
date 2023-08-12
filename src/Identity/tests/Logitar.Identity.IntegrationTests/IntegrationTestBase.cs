using Logitar.Data;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity;

public abstract class IntegrationTestBase
{
  protected IntegrationTestBase()
  {
    IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
      .Build();


    IServiceCollection services = new ServiceCollection()
      .AddSingleton(configuration)
      .Configure<RoleSettings>(options => { })
      .Configure<UserSettings>(options => { });

    string connectionString;
    DatabaseProvider databaseProvider = configuration.GetValue<DatabaseProvider>("DatabaseProvider");
    switch (databaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCorePostgreSQL:
        connectionString = (configuration.GetValue<string>("POSTGRESQLCONNSTR_IntegrationTests") ?? string.Empty)
          .Replace("{database}", $"Identity_{GetType().Name}");
        services.AddLogitarIdentityWithEntityFrameworkCorePostgreSQL(connectionString);
        break;
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        connectionString = (configuration.GetValue<string>("SQLCONNSTR_IntegrationTests") ?? string.Empty)
          .Replace("{database}", $"Identity_{GetType().Name}");
        services.AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString);
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }

    ServiceProvider = services.BuildServiceProvider();

    EventContext = ServiceProvider.GetRequiredService<EventContext>();
    IdentityContext = ServiceProvider.GetRequiredService<IdentityContext>();
  }

  protected IServiceProvider ServiceProvider { get; }
  protected EventContext EventContext { get; }
  protected IdentityContext IdentityContext { get; }

  public virtual async Task InitializeAsync()
  {
    ISqlHelper sql = ServiceProvider.GetRequiredService<ISqlHelper>();
    ICommand delete;

    await EventContext.Database.MigrateAsync();
    delete = sql.DeleteFrom(Db.Events.Table).Build();
    await EventContext.Database.ExecuteSqlRawAsync(delete.Text, delete.Parameters.ToArray());

    await IdentityContext.Database.MigrateAsync();
    foreach (TableId table in new[] { Db.Sessions.Table, Db.Users.Table, Db.Roles.Table })
    {
      delete = sql.DeleteFrom(table).Build();
      await IdentityContext.Database.ExecuteSqlRawAsync(delete.Text, delete.Parameters.ToArray());
    }
  }

  public virtual Task DisposeAsync() => Task.CompletedTask;

  protected static void AssertEqual(DateTime? left, DateTime? right)
  {
    if (left == null || right == null)
    {
      Assert.Equal(left, right);
    }
    else
    {
      Assert.Equal(ToUnixTimeMilliseconds(left.Value), ToUnixTimeMilliseconds(right.Value));
    }
  }
  private static long ToUnixTimeMilliseconds(DateTime moment)
    => new DateTimeOffset(moment).ToUnixTimeMilliseconds();
}
