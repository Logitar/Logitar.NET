using Logitar.EventSourcing;
using Logitar.Identity.Core.Models;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;

public class ActorService : IActorService
{
  private static readonly Actor _system = new();

  public Task<ActorEntity> FindAsync(DomainEvent change, CancellationToken cancellationToken)
  {
    if (change.ActorId == _system.Id)
    {
      return Task.FromResult(ActorEntity.From(_system));
    }

    throw new NotImplementedException(); // TODO(fpion): implement
  }
}
