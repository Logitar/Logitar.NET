using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Handlers.Sessions;

public class SessionRenewedEventHandler : INotificationHandler<SessionRenewedEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public SessionRenewedEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _context = context;
  }

  /// <summary>
  /// TODO(fpion): should be idempotent
  /// </summary>
  /// <param name="notification"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public async Task Handle(SessionRenewedEvent notification, CancellationToken cancellationToken)
  {
    SessionEntity session = await _context.Sessions
      .Include(x => x.User)
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken)
      ?? throw new EntityNotFoundException<UserEntity>(notification.AggregateId.Value);

    ActorEntity actor = (session.User != null && notification.ActorId == session.User.AggregateId)
      ? ActorEntity.From(session.User)
      : await _actorService.FindAsync(notification, cancellationToken);

    session.Renew(notification, actor);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
