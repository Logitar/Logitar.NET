using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Domain;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityDomain(this IServiceCollection services)
  {
    return services
      .AddManagers()
      .AddPasswordStrategies()
      .AddSingleton<IPasswordService, PasswordService>();
  }

  private static IServiceCollection AddManagers(this IServiceCollection services)
  {
    return services
      .AddTransient<IRoleManager, RoleManager>()
      .AddTransient<ISessionManager, SessionManager>()
      .AddTransient<IUserManager, UserManager>();
  }

  private static IServiceCollection AddPasswordStrategies(this IServiceCollection services)
  {
    return services.AddSingleton<IPasswordStrategy, Pbkdf2Strategy>();
  }
}
