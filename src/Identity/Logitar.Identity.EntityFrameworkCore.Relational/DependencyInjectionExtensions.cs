﻿using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Core;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Tokens;
using Logitar.Identity.Core.Users;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Converters;
using Logitar.Identity.EntityFrameworkCore.Relational.Queriers;
using Logitar.Identity.EntityFrameworkCore.Relational.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddLogitarIdentityWithEntityFrameworkCoreRelational(this IServiceCollection services)
  {
    Assembly assembly = typeof(DependencyInjectionExtensions).Assembly;

    EventSerializer.Instance.RegisterConverter(new CultureInfoConverter());
    EventSerializer.Instance.RegisterConverter(new GenderConverter());
    EventSerializer.Instance.RegisterConverter(new PasswordConverter());
    EventSerializer.Instance.RegisterConverter(new TimeZoneEntryConverter());

    return services
      .AddAutoMapper(assembly)
      .AddLogitarIdentityCore()
      .AddMediatR(config => config.RegisterServicesFromAssembly(assembly))
      .AddQueriers()
      .AddRepositories()
      .AddScoped<IActorService, ActorService>()
      .AddScoped<IEventBus, EventBus>()
      .AddScoped<ITokenBlacklist, TokenBlacklist>()
      .AddSingleton<ICurrentActor, SystemActor>();
  }

  private static IServiceCollection AddQueriers(this IServiceCollection services)
  {
    return services
      .AddScoped<IApiKeyQuerier, ApiKeyQuerier>()
      .AddScoped<IRoleQuerier, RoleQuerier>()
      .AddScoped<ISessionQuerier, SessionQuerier>()
      .AddScoped<IUserQuerier, UserQuerier>();
  }

  private static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    return services
      .AddScoped<IApiKeyRepository, ApiKeyRepository>()
      .AddScoped<IRoleRepository, RoleRepository>()
      .AddScoped<ISessionRepository, SessionRepository>()
      .AddScoped<IUserRepository, UserRepository>();
  }
}
