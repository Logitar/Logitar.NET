using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Sessions;

public class SessionUpdatedEventHandler : INotificationHandler<SessionUpdatedEvent>
{
  private readonly IdentityContext _context;

  public SessionUpdatedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(SessionUpdatedEvent notification, CancellationToken cancellationToken)
  {
    SessionEntity? session = await _context.Sessions.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (session != null)
    {
      session.Update(notification);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
