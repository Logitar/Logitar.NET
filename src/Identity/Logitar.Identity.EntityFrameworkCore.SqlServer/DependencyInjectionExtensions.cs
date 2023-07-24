using Logitar.EventSourcing.EntityFrameworkCore.SqlServer;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Tokens;
using Logitar.Identity.Core.Users;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Converters;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Queries;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityWithEntityFrameworkCoreSqlServer(this IServiceCollection services, string connectionString)
  {
    Assembly assembly = typeof(DependencyInjectionExtensions).Assembly;

    EventSerializer.Instance.RegisterConverter(new CultureInfoConverter());
    EventSerializer.Instance.RegisterConverter(new Pbkdf2Converter());

    return services
      .AddAutoMapper(assembly)
      .AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionString))
      .AddLogitarEventSourcingWithEntityFrameworkCoreSqlServer(connectionString)
      .AddLogitarIdentityCore()
      .AddMediatR(config => config.RegisterServicesFromAssembly(assembly))
      .AddQueriers()
      .AddRepositories()
      .AddScoped<IActorService, ActorService>()
      .AddScoped<IEventBus, EventBus>()
      .AddScoped<ITokenBlacklist, TokenBlacklist>();
  }

  private static IServiceCollection AddQueriers(this IServiceCollection services)
  {
    return services
      .AddScoped<ISessionQuerier, SessionQuerier>()
      .AddScoped<IUserQuerier, UserQuerier>();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services
      .AddScoped<ISessionRepository, SessionRepository>()
      .AddScoped<IUserRepository, UserRepository>();
  }
}
