using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;

public class UserAuthenticatedEventHandler : INotificationHandler<UserAuthenticatedEvent>
{
  private readonly IdentityContext _context;

  public UserAuthenticatedEventHandler(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(UserAuthenticatedEvent notification, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken)
      ?? throw new EntityNotFoundException<UserEntity>(notification.AggregateId.Value);

    user.Authenticate(notification);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
