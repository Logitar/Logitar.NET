using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Handlers.Users;

public class UserDeletedEventHandler : INotificationHandler<UserDeletedEvent>
{
  private readonly IdentityContext _context;

  public UserDeletedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
  {
    UserEntity? user = await _context.Users
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (user == null)
    {
      return;
    }

    _context.Users.Remove(user);
    await _context.SaveChangesAsync(cancellationToken);
  }
}
