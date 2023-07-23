using Logitar.EventSourcing;
using Logitar.Identity.Core.Models;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;

public class ActorService : IActorService
{
  private static readonly Actor _system = new();

  private readonly IdentityContext _context;

  public ActorService(IdentityContext context)
  {
    _context = context;
  }

  public async Task<ActorEntity> FindAsync(DomainEvent change, CancellationToken cancellationToken)
  {
    if (change.ActorId == null || change.ActorId == _system.Id)
    {
      return ActorEntity.From(_system);
    }

    UserEntity? user = await _context.Users.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == change.ActorId, cancellationToken);
    if (user != null)
    {
      return ActorEntity.From(user);
    }

    // TODO(fpion): API key actors

    throw new ActorNotFoundException(change.ActorId);
  }
}
