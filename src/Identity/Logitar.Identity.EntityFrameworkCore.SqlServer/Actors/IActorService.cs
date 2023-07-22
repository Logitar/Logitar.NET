using Logitar.EventSourcing;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;

public interface IActorService
{
  Task<ActorEntity> FindAsync(DomainEvent change, CancellationToken cancellationToken = default);
}
