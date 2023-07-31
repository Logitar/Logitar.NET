using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Roles;

public class RoleUpdatedEventHandler : INotificationHandler<RoleUpdatedEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public RoleUpdatedEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _context = context;
  }

  public async Task Handle(RoleUpdatedEvent notification, CancellationToken cancellationToken)
  {
    ActorEntity actor = await _actorService.FindAsync(notification, cancellationToken);
    RoleEntity role = await _context.Roles
      .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken)
      ?? throw new EntityNotFoundException<RoleEntity>(notification.AggregateId.Value);

    role.Update(notification, actor);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
