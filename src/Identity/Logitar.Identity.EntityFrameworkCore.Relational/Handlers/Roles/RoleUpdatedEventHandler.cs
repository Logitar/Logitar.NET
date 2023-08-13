using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Roles;

public class RoleUpdatedEventHandler : INotificationHandler<RoleUpdatedEvent>
{
  private readonly IdentityContext _context;

  public RoleUpdatedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(RoleUpdatedEvent notification, CancellationToken cancellationToken)
  {
    RoleEntity? role = await _context.Roles
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (role != null)
    {
      role.Update(notification);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
