using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Sessions;

public class SessionRenewedEventHandler : INotificationHandler<SessionRenewedEvent>
{
  private readonly IdentityContext _context;

  public SessionRenewedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(SessionRenewedEvent notification, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _context.Sessions.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (session != null)
    {
      session.Renew(notification);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
