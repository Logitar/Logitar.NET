using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Repositories;
using Logitar.Identity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityWithEntityFrameworkCoreRelational(this IServiceCollection services)
  {
    Assembly assembly = typeof(DependencyInjectionExtensions).Assembly;

    return services
      .AddLogitarEventSourcingWithEntityFrameworkCoreRelational()
      .AddLogitarIdentityInfrastructure()
      .AddMediatR(config => config.RegisterServicesFromAssembly(assembly))
      .AddRepositories();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services
      .AddScoped<IRoleRepository, RoleRepository>()
      .AddScoped<ISessionRepository, SessionRepository>()
      .AddScoped<IUserRepository, UserRepository>();
  }
}
