using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;

public class UserEnabledEventHandler : INotificationHandler<UserEnabledEvent>
{
  private readonly IdentityContext _context;

  public UserEnabledEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(UserEnabledEvent notification, CancellationToken cancellationToken)
  {
    UserEntity? user = await _context.Users.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (user != null)
    {
      user.Enable(notification);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
