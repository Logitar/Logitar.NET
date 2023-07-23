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
      .AddTransient<ITokenManager, JsonWebTokenManager>()
      .AddTransient<ITokenService, TokenService>()
      .AddTransient<IUserService, UserService>();
  }
}
