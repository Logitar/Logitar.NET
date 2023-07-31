using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Sessions;

public class SessionUpdatedEventHandler : INotificationHandler<SessionUpdatedEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public SessionUpdatedEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _context = context;
  }

  public async Task Handle(SessionUpdatedEvent notification, CancellationToken cancellationToken)
  {
    ActorEntity actor = await _actorService.FindAsync(notification, cancellationToken);
    SessionEntity session = await _context.Sessions
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken)
      ?? throw new EntityNotFoundException<SessionEntity>(notification.AggregateId.Value);

    session.Update(notification, actor);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
