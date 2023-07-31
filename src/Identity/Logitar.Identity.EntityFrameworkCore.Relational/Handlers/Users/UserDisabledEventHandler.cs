using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Users;

public class UserDisabledEventHandler : INotificationHandler<UserDisabledEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public UserDisabledEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _context = context;
  }

  public async Task Handle(UserDisabledEvent notification, CancellationToken cancellationToken)
  {
    ActorEntity actor = await _actorService.FindAsync(notification, cancellationToken);
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken)
      ?? throw new EntityNotFoundException<UserEntity>(notification.AggregateId.Value);

    user.Disable(notification, actor);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
