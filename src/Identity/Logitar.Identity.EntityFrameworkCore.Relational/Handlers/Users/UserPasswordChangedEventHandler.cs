using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;

public class UserPasswordChangedEventHandler : INotificationHandler<UserPasswordChangedEvent>
{
  private readonly IdentityContext _context;

  public UserPasswordChangedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(UserPasswordChangedEvent notification, CancellationToken cancellationToken)
  {
    UserEntity? user = await _context.Users
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (user != null)
    {
      user.ChangePassword(notification);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
