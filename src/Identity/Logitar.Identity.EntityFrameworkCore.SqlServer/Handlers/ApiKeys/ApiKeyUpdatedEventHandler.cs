﻿using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Handlers.ApiKeys;

public class ApiKeyUpdatedEventHandler : INotificationHandler<ApiKeyUpdatedEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public ApiKeyUpdatedEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _context = context;
  }

  public async Task Handle(ApiKeyUpdatedEvent notification, CancellationToken cancellationToken)
  {
    ActorEntity actor = await _actorService.FindAsync(notification, cancellationToken);
    ApiKeyEntity apiKey = await _context.ApiKeys
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken)
      ?? throw new EntityNotFoundException<ApiKeyEntity>(notification.AggregateId.Value);

    apiKey.Update(notification, actor);

    await _context.SaveChangesAsync(cancellationToken);

    if (notification.Title != null)
    {
      await _actorService.UpdateAsync(apiKey, cancellationToken);
    }
  }
}
