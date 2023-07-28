using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

public class RoleRepository : IdentityRepository, IRoleRepository
{
  private static readonly string _aggregateType = typeof(RoleAggregate).GetName();

  public RoleRepository(ICurrentActor currentActor, IEventBus eventBus, EventContext eventContext)
    : base(currentActor, eventBus, eventContext)
  {
  }

  public async Task<RoleAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken)
    => await base.LoadAsync<RoleAggregate>(id, cancellationToken);

  public async Task<IEnumerable<RoleAggregate>> LoadAsync(string? tenantId, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();

    IQuery query = SqlServerQueryBuilder.From(Db.Events.Table)
      .Join(Db.Roles.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(_aggregateType))
      )
      .Where(new OperatorCondition(Db.Roles.TenantId,
        tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId)))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .OrderBy(e => e.Version)
      .AsNoTracking()
      .ToArrayAsync(cancellationToken);

    return Load<RoleAggregate>(events.Select(EventSerializer.Instance.Deserialize));
  }
  public async Task<RoleAggregate?> LoadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string uniqueNameNormalized = uniqueName.Trim().ToUpper();

    IQuery query = SqlServerQueryBuilder.From(Db.Events.Table)
      .Join(Db.Roles.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(_aggregateType))
      )
      .WhereAnd(
        new OperatorCondition(Db.Roles.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId)),
        new OperatorCondition(Db.Roles.UniqueNameNormalized, Operators.IsEqualTo(uniqueNameNormalized))
      )
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .OrderBy(e => e.Version)
      .AsNoTracking()
      .ToArrayAsync(cancellationToken);

    return Load<RoleAggregate>(events.Select(EventSerializer.Instance.Deserialize)).SingleOrDefault();
  }

  public async Task SaveAsync(RoleAggregate role, CancellationToken cancellationToken)
    => await base.SaveAsync(role, cancellationToken);
  public async Task SaveAsync(IEnumerable<RoleAggregate> roles, CancellationToken cancellationToken)
    => await base.SaveAsync(roles, cancellationToken);
}
