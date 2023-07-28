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
      .AddApplicationFacades()
      .AddApplicationServices()
      .AddMediatR(config => config.RegisterServicesFromAssembly(assembly))
      .AddTransient<ITokenManager, JsonWebTokenManager>();
  }

  private static IServiceCollection AddApplicationFacades(this IServiceCollection services)
  {
    return services
      .AddTransient<IApiKeyFacade, ApiKeyFacade>()
      .AddTransient<IRoleFacade, RoleFacade>()
      .AddTransient<ISessionFacade, SessionFacade>()
      .AddTransient<ITokenFacade, TokenFacade>();
  }
  private static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    return services.AddTransient<IUserService, UserService>();
  }
}
