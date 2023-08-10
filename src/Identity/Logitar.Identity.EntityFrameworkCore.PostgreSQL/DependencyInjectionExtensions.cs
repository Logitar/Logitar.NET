using Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityWithEntityFrameworkCorePostgreSQL(this IServiceCollection services, string connectionString)
  {
    return services
      .AddDbContext<IdentityContext>(options => options.UseNpgsql(connectionString,
        options => options.MigrationsAssembly("Logitar.Identity.EntityFrameworkCore.PostgreSQL")
      ))
      .AddLogitarEventSourcingWithEntityFrameworkCorePostgreSQL(connectionString)
      .AddLogitarIdentityWithEntityFrameworkCoreRelational()
      .AddSingleton<ISqlHelper, SqlHelper>();
  }
}
