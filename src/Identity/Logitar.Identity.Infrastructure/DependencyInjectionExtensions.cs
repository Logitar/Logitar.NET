using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Infrastructure;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityInfrastructure(this IServiceCollection services)
  {
    return services
      .AddLogitarIdentityDomain()
      .AddScoped<IEventBus, EventBus>()
      .AddSingleton<PasswordConverter>()
      .AddSingleton<IEventSerializer>(serviceProvider => new EventSerializer(new JsonConverter[]
      {
        new GenderConverter(),
        new LocaleConverter(),
        new TimeZoneEntryConverter(),
        serviceProvider.GetRequiredService<PasswordConverter>()
      }));
  }
}
