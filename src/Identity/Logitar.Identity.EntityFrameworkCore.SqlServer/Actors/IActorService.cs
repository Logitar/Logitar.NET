using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Actors;

public interface IActorService
{
  Task<ActorEntity> FindAsync(DomainEvent change, CancellationToken cancellationToken = default);

  Task DeleteAsync(ApiKeyEntity apiKey, CancellationToken cancellationToken = default);
  Task UpdateAsync(ApiKeyEntity apiKey, CancellationToken cancellationToken = default);

  Task DeleteAsync(UserEntity user, CancellationToken cancellationToken = default);
  Task UpdateAsync(UserEntity user, CancellationToken cancellationToken = default);
}
