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

  /// <summary>
  /// TODO(fpion): should be idempotent
  /// </summary>
  /// <param name="notification"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken)
      ?? throw new EntityNotFoundException<UserEntity>(notification.AggregateId.Value);

    _context.Users.Remove(user);
    await _context.SaveChangesAsync(cancellationToken);
  }
}
