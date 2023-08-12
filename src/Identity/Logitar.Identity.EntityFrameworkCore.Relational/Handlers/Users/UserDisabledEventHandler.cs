using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;

public class UserDisabledEventHandler : INotificationHandler<UserDisabledEvent>
{
  private readonly IdentityContext _context;

  public UserDisabledEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(UserDisabledEvent notification, CancellationToken cancellationToken)
  {
    UserEntity? user = await _context.Users
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (user != null)
    {
      user.Disable(notification);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
