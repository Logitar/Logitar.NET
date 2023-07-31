using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Actors;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Roles;

public class RoleCreatedEventHandler : INotificationHandler<RoleCreatedEvent>
{
  private readonly IActorService _actorService;
  private readonly IdentityContext _context;

  public RoleCreatedEventHandler(IActorService actorService, IdentityContext context)
  {
    _actorService = actorService;
    _context = context;
  }

  public async Task Handle(RoleCreatedEvent notification, CancellationToken cancellationToken)
  {
    bool exists = await _context.Roles.AnyAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
    if (exists)
    {
      return;
    }

    ActorEntity actor = await _actorService.FindAsync(notification, cancellationToken);
    RoleEntity role = new(notification, actor);

    _context.Roles.Add(role);
    await _context.SaveChangesAsync(cancellationToken);
  }
}
