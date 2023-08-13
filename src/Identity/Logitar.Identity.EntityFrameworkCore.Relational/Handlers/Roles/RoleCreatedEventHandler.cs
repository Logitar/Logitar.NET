using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Roles;

public class RoleCreatedEventHandler : INotificationHandler<RoleCreatedEvent>
{
  private readonly IdentityContext _context;

  public RoleCreatedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(RoleCreatedEvent notification, CancellationToken cancellationToken)
  {
    RoleEntity? role = await _context.Roles.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (role == null)
    {
      role = new RoleEntity(notification);

      _context.Roles.Add(role);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
