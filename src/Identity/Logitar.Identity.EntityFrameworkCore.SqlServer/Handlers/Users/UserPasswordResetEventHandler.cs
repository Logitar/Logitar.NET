using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Handlers.Users;

public class UserPasswordResetEventHandler : INotificationHandler<UserPasswordResetEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public UserPasswordResetEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _context = context;
  }

  public async Task Handle(UserPasswordResetEvent notification, CancellationToken cancellationToken)
  {
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken)
      ?? throw new EntityNotFoundException<UserEntity>(notification.AggregateId.Value);
    ActorEntity actor = notification.ActorId == user.AggregateId
      ? ActorEntity.From(user)
      : await _actorService.FindAsync(notification, cancellationToken);

    user.ResetPassword(notification, actor);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
