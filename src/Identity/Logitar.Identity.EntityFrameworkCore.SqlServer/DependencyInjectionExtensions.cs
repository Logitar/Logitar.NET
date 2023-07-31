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
        b => b.MigrationsAssembly("Logitar.Identity.EntityFrameworkCore.SqlServer")))
      .AddLogitarEventSourcingWithEntityFrameworkCoreSqlServer(connectionString)
      .AddLogitarIdentityWithEntityFrameworkCoreRelational()
      .AddSingleton<IQueryHelper, QueryHelper>();
  }
}
