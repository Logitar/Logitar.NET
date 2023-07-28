using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

public class ApiKeyRepository : IdentityRepository, IApiKeyRepository
{
  private static readonly string _aggregateType = typeof(ApiKeyAggregate).GetName();

  public ApiKeyRepository(ICurrentActor currentActor, IEventBus eventBus, EventContext eventContext)
    : base(currentActor, eventBus, eventContext)
  {
  }

  public async Task<ApiKeyAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken)
    => await base.LoadAsync<ApiKeyAggregate>(id, cancellationToken);

  public async Task<IEnumerable<ApiKeyAggregate>> LoadAsync(RoleAggregate role, CancellationToken cancellationToken)
  {
    IQuery query = SqlServerQueryBuilder.From(Db.Events.Table)
      .Join(Db.ApiKeys.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(_aggregateType))
      )
      .Join(Db.ApiKeyRoles.ApiKeyId, Db.ApiKeys.ApiKeyId)
      .Join(Db.Roles.RoleId, Db.ApiKeyRoles.RoleId)
      .Where(Db.Roles.AggregateId, Operators.IsEqualTo(role.Id.Value))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .OrderBy(e => e.Version)
      .AsNoTracking()
      .ToArrayAsync(cancellationToken);

    return Load<ApiKeyAggregate>(events.Select(EventSerializer.Instance.Deserialize));
  }

  public async Task SaveAsync(ApiKeyAggregate apiKey, CancellationToken cancellationToken)
    => await base.SaveAsync(apiKey, cancellationToken);
  public async Task SaveAsync(IEnumerable<ApiKeyAggregate> apiKeys, CancellationToken cancellationToken)
    => await base.SaveAsync(apiKeys, cancellationToken);
}
