using Logitar.Identity.Core.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Core;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityCore(this IServiceCollection services)
  {
    return services.AddTransient<IUserService, UserService>();
  }
}
