using Logitar.EventSourcing.EntityFrameworkCore.SqlServer;
using Logitar.EventSourcing.Infrastructure;

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

    services.AddControllers();

    if (_enableOpenApi)
    {
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();
    }

    //string connectionString = _configuration.GetValue<string>("POSTGRESQLCONNSTR_Demo") ?? string.Empty;
    //services.AddLogitarEventSourcingWithEntityFrameworkCorePostgreSQL(connectionString);
    string connectionString = _configuration.GetValue<string>("SQLCONNSTR_Demo") ?? string.Empty;
    services.AddLogitarEventSourcingWithEntityFrameworkCoreSqlServer(connectionString);

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
