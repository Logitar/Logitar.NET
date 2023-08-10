using Logitar.EventSourcing.EntityFrameworkCore.SqlServer;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityWithEntityFrameworkCoreSqlServer(this IServiceCollection services, string connectionString)
  {
    return services
      .AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionString,
        options => options.MigrationsAssembly("Logitar.Identity.EntityFrameworkCore.SqlServer")
      ))
      .AddLogitarEventSourcingWithEntityFrameworkCoreSqlServer(connectionString)
      .AddLogitarIdentityWithEntityFrameworkCoreRelational()
      .AddSingleton<ISqlHelper, SqlHelper>();
  }
}
