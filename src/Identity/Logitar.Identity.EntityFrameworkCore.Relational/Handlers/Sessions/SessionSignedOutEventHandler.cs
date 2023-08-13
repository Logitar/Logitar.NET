using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Sessions;

public class SessionSignedOutEventHandler : INotificationHandler<SessionSignedOutEvent>
{
  private readonly IdentityContext _context;

  public SessionSignedOutEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(SessionSignedOutEvent notification, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _context.Sessions
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (session != null)
    {
      session.SignOut(notification);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
