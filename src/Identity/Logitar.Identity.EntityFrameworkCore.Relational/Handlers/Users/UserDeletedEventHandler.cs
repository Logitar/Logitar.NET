using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;

public class UserDeletedEventHandler : INotificationHandler<UserDeletedEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public UserDeletedEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
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

    await _actorService.DeleteAsync(user, cancellationToken);
  }
}
