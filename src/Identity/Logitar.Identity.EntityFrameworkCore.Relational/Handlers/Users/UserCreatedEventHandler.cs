using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
  private readonly IdentityContext _context;

  public UserCreatedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
  {
    UserEntity? user = await _context.Users.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (user == null)
    {
      user = new UserEntity(notification);

      _context.Users.Add(user);
      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
