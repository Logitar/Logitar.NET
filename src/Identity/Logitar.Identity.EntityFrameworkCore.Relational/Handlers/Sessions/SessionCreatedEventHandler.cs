using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Sessions;

public class SessionCreatedEventHandler : INotificationHandler<SessionCreatedEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public SessionCreatedEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _context = context;
  }

  public async Task Handle(SessionCreatedEvent notification, CancellationToken cancellationToken)
  {
    bool exists = await _context.Sessions.AnyAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (exists)
    {
      return;
    }

    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.AggregateId == notification.UserId.Value, cancellationToken)
      ?? throw new EntityNotFoundException<UserEntity>(notification.UserId.Value);

    ActorEntity actor = await _actorService.FindAsync(notification, cancellationToken);
    SessionEntity session = new(notification, actor, user);

    _context.Sessions.Add(session);
    await _context.SaveChangesAsync(cancellationToken);
  }
}
