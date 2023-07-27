using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Domain.ApiKeys;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

public class ApiKeyRepository : IdentityRepository, IApiKeyRepository
{
  public ApiKeyRepository(ICurrentActor currentActor, IEventBus eventBus, EventContext eventContext)
    : base(currentActor, eventBus, eventContext)
  {
  }

  public async Task<ApiKeyAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken)
    => await base.LoadAsync<ApiKeyAggregate>(id, cancellationToken);

  public async Task SaveAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken)
    => await base.SaveAsync(apiKey, cancellationToken);
  public async Task SaveAsync(IEnumerable<ApiKeyAggregate> apiKeys, CancellationToken cancellationToken)
    => await base.SaveAsync(apiKeys, cancellationToken);
}
