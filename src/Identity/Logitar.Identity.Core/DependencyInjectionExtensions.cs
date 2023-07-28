using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Roles.Commands;
using Logitar.Identity.Core.Roles.Queries;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Sessions.Commands;
using Logitar.Identity.Core.Tokens;
using Logitar.Identity.Core.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Core;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityCore(this IServiceCollection services)
  {
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

    return services
      .AddApplicationServices()
      .AddCommands()
      .AddQueries()
      .AddTransient<ITokenManager, JsonWebTokenManager>();
  }

  private static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    return services
      .AddTransient<IApiKeyService, ApiKeyService>()
      .AddTransient<IRoleService, RoleService>()
      .AddTransient<ISessionService, SessionService>()
      .AddTransient<ITokenService, TokenService>()
      .AddTransient<IUserService, UserService>();
  }

  private static IServiceCollection AddCommands(this IServiceCollection services)
  {
    return services
      .AddTransient<IDeleteRoleCommand, DeleteRoleCommandHandler>()
      .AddTransient<IDeleteSessionsCommand, DeleteSessionsCommandHandler>();
  }

  private static IServiceCollection AddQueries(this IServiceCollection services)
  {
    return services.AddTransient<IFindRolesQuery, FindRolesQueryHandler>();
  }
}
