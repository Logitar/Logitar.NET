using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Roles;

public class RoleDeletedEventHandler : INotificationHandler<RoleDeletedEvent>
{
  private readonly IdentityContext _context;

  public RoleDeletedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(RoleDeletedEvent notification, CancellationToken cancellationToken)
  {
    RoleEntity? role = await _context.Roles
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (role == null)
    {
      return;
    }

    _context.Roles.Remove(role);
    await _context.SaveChangesAsync(cancellationToken);
  }
}
