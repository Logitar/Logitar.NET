using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Handlers.Users;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public UserCreatedEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _context = context;
  }

  public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
  {
    bool exists = await _context.Users.AnyAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (exists)
    {
      return;
    }

    ActorEntity actor = await _actorService.FindAsync(notification, cancellationToken);
    UserEntity user = new(notification, actor);

    _context.Users.Add(user);
    await _context.SaveChangesAsync(cancellationToken);
  }
}
