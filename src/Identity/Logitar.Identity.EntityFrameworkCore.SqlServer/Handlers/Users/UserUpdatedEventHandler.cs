using Logitar.Identity.Domain.Users.Events;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Handlers.Users;

public class UserUpdatedEventHandler : INotificationHandler<UserUpdatedEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public UserUpdatedEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _context = context;
  }

  public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
  {
    ActorEntity actor = await _actorService.FindAsync(notification, cancellationToken);
    UserEntity user = await _context.Users
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken)
      ?? throw new EntityNotFoundException<UserEntity>(notification.AggregateId.Value);

    IEnumerable<string> roleIds = notification.Roles.Select(x => x.Key);
    RoleEntity[] roles = await _context.Roles
      .Where(x => roleIds.Contains(x.AggregateId))
      .ToArrayAsync(cancellationToken);

    user.Update(notification, actor, roles);

    await _context.SaveChangesAsync(cancellationToken);

    if (notification.FullName != null || notification.Email != null || notification.Picture != null)
    {
      await _actorService.UpdateAsync(user, cancellationToken);
    }
  }
}
