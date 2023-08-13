using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Sessions;

public class SessionCreatedEventHandler : INotificationHandler<SessionCreatedEvent>
{
  private readonly IdentityContext _context;

  public SessionCreatedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(SessionCreatedEvent notification, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _context.Sessions.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (session == null)
    {
      UserEntity? user = await _context.Users
        .SingleOrDefaultAsync(x => x.AggregateId == notification.UserId.Value, cancellationToken);
      if (user != null)
      {
        session = new SessionEntity(notification, user);

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync(cancellationToken);
      }
    }
  }
}
