using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Roles;
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
      .AddTransient<IApiKeyService, ApiKeyService>()
      .AddTransient<IDeleteSessionsCommand, DeleteSessionsCommandHandler>()
      .AddTransient<IRoleService, RoleService>()
      .AddTransient<ISessionService, SessionService>()
      .AddTransient<ITokenManager, JsonWebTokenManager>()
      .AddTransient<ITokenService, TokenService>()
      .AddTransient<IUserService, UserService>();
  }
}
