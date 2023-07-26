using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Handlers.Sessions;

public class SessionDeletedEventHandler : INotificationHandler<SessionDeletedEvent>
{
  private readonly IdentityContext _context;

  public SessionDeletedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(SessionDeletedEvent notification, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _context.Sessions
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (session == null)
    {
      return;
    }

    _context.Sessions.Remove(session);
    await _context.SaveChangesAsync(cancellationToken);
  }
}
