using Logitar.EventSourcing.Infrastructure;
using Logitar.EventSourcing.InMemory;
using Logitar.EventSourcing.MongoDB;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL;
using Logitar.Identity.EntityFrameworkCore.SqlServer;

namespace Logitar.Demo.Ui;

internal class Startup : StartupBase
{
  private readonly IConfiguration _configuration;

  private readonly bool _enableOpenApi;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;

    _enableOpenApi = configuration.GetValue<bool>("EnableOpenApi");
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    services.AddControllers()
      .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    if (_enableOpenApi)
    {
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();
    }

    DatabaseProvider databaseProvider = _configuration.GetValue<DatabaseProvider?>("DatabaseProvider")
      ?? DatabaseProvider.EntityFrameworkCoreSqlServer;
    string connectionString;
    switch (databaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCorePostgreSQL:
        connectionString = _configuration.GetValue<string>("POSTGRESQLCONNSTR_Demo") ?? string.Empty;
        services.AddLogitarIdentityWithEntityFrameworkCorePostgreSQL(connectionString);
        break;
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        connectionString = _configuration.GetValue<string>("SQLCONNSTR_Demo") ?? string.Empty;
        services.AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString);
        break;
      case DatabaseProvider.InMemory:
        services.AddLogitarEventSourcingInMemory();
        break;
      case DatabaseProvider.MongoDB:
        services.AddLogitarEventSourcingMongoDB(_configuration);
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }

    services.AddSingleton<IEventBus, EventBus>();
  }

  public override void Configure(IApplicationBuilder builder)
  {
    if (_enableOpenApi)
    {
      builder.UseSwagger();
      builder.UseSwaggerUI();
    }

    builder.UseHttpsRedirection();
    builder.UseStaticFiles();
    builder.UseAuthentication();
    builder.UseAuthorization();

    if (builder is WebApplication application)
    {
      application.MapControllers();
    }
  }
}
