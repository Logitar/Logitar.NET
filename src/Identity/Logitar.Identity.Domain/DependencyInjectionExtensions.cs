using Logitar.Identity.Domain.Passwords;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Domain;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityDomain(this IServiceCollection services)
  {
    return services
      .AddPasswordStrategies()
      .AddSingleton<IPasswordService, PasswordService>();
  }

  private static IServiceCollection AddPasswordStrategies(this IServiceCollection services)
  {
    return services.AddSingleton<IPasswordStrategy, Pbkdf2Strategy>();
  }
}
