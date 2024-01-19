namespace Logitar.Demo;

internal class Startup : StartupBase
{
  private readonly bool _enableOpenApi;

  public Startup(IConfiguration configuration)
  {
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
  }

  public override void Configure(IApplicationBuilder builder)
  {
    if (_enableOpenApi)
    {
      builder.UseSwagger();
      builder.UseSwaggerUI();
    }

    builder.UseHttpsRedirection();

    if (builder is WebApplication application)
    {
      application.MapControllers();
    }
  }
}
