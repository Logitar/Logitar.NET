using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Handlers.Users;

public class UserSignedInEventHandler : INotificationHandler<UserSignedInEvent>
{
  private readonly IdentityContext _context;

  public UserSignedInEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(UserSignedInEvent notification, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken)
      ?? throw new EntityNotFoundException<UserEntity>(notification.AggregateId.Value);

    user.SignIn(notification);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
