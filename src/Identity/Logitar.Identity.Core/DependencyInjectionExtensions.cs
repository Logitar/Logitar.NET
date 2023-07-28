using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Roles.Commands;
using Logitar.Identity.Core.Roles.Queries;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Tokens;
using Logitar.Identity.Core.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Logitar.Identity.Core;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityCore(this IServiceCollection services)
  {
    Assembly assembly = typeof(DependencyInjectionExtensions).Assembly;

    return services
      .AddApplicationFacades()
      .AddApplicationServices()
      .AddCommands()
      .AddMediatR(config => config.RegisterServicesFromAssembly(assembly))
      .AddQueries()
      .AddTransient<ITokenManager, JsonWebTokenManager>();
  }

  private static IServiceCollection AddApplicationFacades(this IServiceCollection services)
  {
    return services
      .AddTransient<ISessionFacade, SessionFacade>()
      .AddTransient<ITokenFacade, TokenFacade>();
  }
  private static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    return services
      .AddTransient<IApiKeyService, ApiKeyService>()
      .AddTransient<IRoleService, RoleService>()
      .AddTransient<IUserService, UserService>();
  }

  private static IServiceCollection AddCommands(this IServiceCollection services)
  {
    return services.AddTransient<IDeleteRoleCommand, DeleteRoleCommandHandler>();
  }

  private static IServiceCollection AddQueries(this IServiceCollection services)
  {
    return services.AddTransient<IFindRolesQuery, FindRolesQueryHandler>();
  }
}
