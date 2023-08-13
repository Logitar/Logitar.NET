using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class RoleRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IRoleRepository
{
  public RoleRepository(IEventBus eventBus, EventContext eventContext,
    IEventSerializer eventSerializer, ISqlHelper sqlHelper)
      : base(eventBus, eventContext, eventSerializer)
  {
    Sql = sqlHelper;
  }

  protected string AggregateType { get; } = typeof(RoleAggregate).GetName();
  protected ISqlHelper Sql { get; private set; }

  public async Task<RoleAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken)
    => await base.LoadAsync<RoleAggregate>(id, cancellationToken);
  public async Task<RoleAggregate?> LoadAsync(AggregateId id, long? version, CancellationToken cancellationToken)
    => await base.LoadAsync<RoleAggregate>(id, version, cancellationToken);
  public async Task<RoleAggregate?> LoadAsync(AggregateId id, bool includeDeleted, CancellationToken cancellationToken)
    => await base.LoadAsync<RoleAggregate>(id, includeDeleted, cancellationToken);
  public async Task<RoleAggregate?> LoadAsync(AggregateId id, long? version, bool includeDeleted, CancellationToken cancellationToken)
    => await base.LoadAsync<RoleAggregate>(id, version, includeDeleted, cancellationToken);

  public async Task<IEnumerable<RoleAggregate>> LoadAsync(CancellationToken cancellationToken)
    => await base.LoadAsync<RoleAggregate>(cancellationToken);
  public async Task<IEnumerable<RoleAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken)
    => await base.LoadAsync<RoleAggregate>(includeDeleted, cancellationToken);

  public async Task<IEnumerable<RoleAggregate>> LoadAsync(IEnumerable<AggregateId> ids, CancellationToken cancellationToken)
    => await base.LoadAsync<RoleAggregate>(ids, cancellationToken);
  public async Task<IEnumerable<RoleAggregate>> LoadAsync(IEnumerable<AggregateId> ids, bool includeDeleted, CancellationToken cancellationToken)
    => await base.LoadAsync<RoleAggregate>(ids, includeDeleted, cancellationToken);

  public async Task<IEnumerable<RoleAggregate>> LoadAsync(string? tenantId, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();

    IQuery query = Sql.QueryFrom(Db.Events.Table)
      .Join(Db.Roles.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(Db.Roles.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return base.Load<RoleAggregate>(events.Select(EventSerializer.Deserialize));
  }
  public async Task<RoleAggregate?> LoadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string uniqueNameNormalized = uniqueName.Trim().ToUpper();

    IQuery query = Sql.QueryFrom(Db.Events.Table)
      .Join(Db.Roles.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(Db.Roles.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId))
      .Where(Db.Roles.UniqueNameNormalized, Operators.IsEqualTo(uniqueNameNormalized))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return base.Load<RoleAggregate>(events.Select(EventSerializer.Deserialize)).SingleOrDefault();
  }
  public async Task<IEnumerable<RoleAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    string aggregateId = user.Id.Value;

    IQuery query = Sql.QueryFrom(Db.Events.Table)
      .Join(Db.Roles.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Join(Db.UserRoles.RoleId, Db.Roles.RoleId)
      .Join(Db.Users.UserId, Db.UserRoles.UserId)
      .Where(Db.Users.AggregateId, Operators.IsEqualTo(aggregateId))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return base.Load<RoleAggregate>(events.Select(EventSerializer.Deserialize));
  }
}
