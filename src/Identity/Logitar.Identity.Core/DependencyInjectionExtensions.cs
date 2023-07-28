using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Roles;
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
      .AddMediatR(config => config.RegisterServicesFromAssembly(assembly))
      .AddTransient<ITokenManager, JsonWebTokenManager>()
      .AddTransient<IApiKeyFacade, ApiKeyFacade>()
      .AddTransient<IRoleFacade, RoleFacade>()
      .AddTransient<ISessionFacade, SessionFacade>()
      .AddTransient<ITokenFacade, TokenFacade>()
      .AddTransient<IUserFacade, UserFacade>();
  }
}
