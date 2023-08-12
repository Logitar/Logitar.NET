using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;

public class UserUpdatedEventHandler : INotificationHandler<UserUpdatedEvent>
{
  private readonly IdentityContext _context;

  public UserUpdatedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
  {
    UserEntity? user = await _context.Users
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (user != null)
    {
      IEnumerable<string> roleIds = notification.Roles.Keys;
      RoleEntity[] roles = await _context.Roles
        .Where(x => roleIds.Contains(x.AggregateId))
        .ToArrayAsync(cancellationToken);

      user.Update(notification, roles);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
