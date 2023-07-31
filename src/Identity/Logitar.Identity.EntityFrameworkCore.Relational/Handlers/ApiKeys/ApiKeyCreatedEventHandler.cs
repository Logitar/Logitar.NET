using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.ApiKeys;

public class ApiKeyCreatedEventHandler : INotificationHandler<ApiKeyCreatedEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public ApiKeyCreatedEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _context = context;
  }

  public async Task Handle(ApiKeyCreatedEvent notification, CancellationToken cancellationToken)
  {
    bool exists = await _context.ApiKeys.AnyAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (exists)
    {
      return;
    }

    ActorEntity actor = await _actorService.FindAsync(notification, cancellationToken);
    ApiKeyEntity apiKey = new(notification, actor);

    _context.ApiKeys.Add(apiKey);
    await _context.SaveChangesAsync(cancellationToken);
  }
}
