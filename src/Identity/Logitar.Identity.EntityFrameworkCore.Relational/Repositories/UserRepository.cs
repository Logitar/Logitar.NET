using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class UserRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IUserRepository
{
  public UserRepository(IEventBus eventBus, EventContext eventContext,
    IEventSerializer eventSerializer, ISqlHelper sqlHelper)
      : base(eventBus, eventContext, eventSerializer)
  {
    Sql = sqlHelper;
  }

  protected string AggregateType { get; } = typeof(UserAggregate).GetName();
  protected ISqlHelper Sql { get; private set; }

  public async Task<UserAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken)
    => await base.LoadAsync<UserAggregate>(id, cancellationToken);
  public async Task<UserAggregate?> LoadAsync(AggregateId id, long? version, CancellationToken cancellationToken)
    => await base.LoadAsync<UserAggregate>(id, version, cancellationToken);
  public async Task<UserAggregate?> LoadAsync(AggregateId id, bool includeDeleted, CancellationToken cancellationToken)
    => await base.LoadAsync<UserAggregate>(id, includeDeleted, cancellationToken);
  public async Task<UserAggregate?> LoadAsync(AggregateId id, long? version, bool includeDeleted, CancellationToken cancellationToken)
    => await base.LoadAsync<UserAggregate>(id, version, includeDeleted, cancellationToken);

  public async Task<IEnumerable<UserAggregate>> LoadAsync(CancellationToken cancellationToken)
    => await base.LoadAsync<UserAggregate>(cancellationToken);
  public async Task<IEnumerable<UserAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken)
    => await base.LoadAsync<UserAggregate>(includeDeleted, cancellationToken);

  public async Task<IEnumerable<UserAggregate>> LoadAsync(IEnumerable<AggregateId> ids, CancellationToken cancellationToken)
    => await base.LoadAsync<UserAggregate>(ids, cancellationToken);
  public async Task<IEnumerable<UserAggregate>> LoadAsync(IEnumerable<AggregateId> ids, bool includeDeleted, CancellationToken cancellationToken)
    => await base.LoadAsync<UserAggregate>(ids, includeDeleted, cancellationToken);

  public async Task<IEnumerable<UserAggregate>> LoadAsync(string? tenantId, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();

    IQuery query = Sql.QueryFrom(Db.Events.Table)
      .Join(Db.Users.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(Db.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return base.Load<UserAggregate>(events.Select(EventSerializer.Deserialize));
  }
  public async Task<UserAggregate?> LoadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string uniqueNameNormalized = uniqueName.Trim().ToUpper();

    IQuery query = Sql.QueryFrom(Db.Events.Table)
      .Join(Db.Users.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(Db.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId))
      .Where(Db.Users.UniqueNameNormalized, Operators.IsEqualTo(uniqueNameNormalized))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return base.Load<UserAggregate>(events.Select(EventSerializer.Deserialize)).SingleOrDefault();
  }
  public async Task<IEnumerable<UserAggregate>> LoadAsync(string? tenantId, IEmailAddress email, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string emailAddressNormalized = email.Address.Trim().ToUpper();

    IQuery query = Sql.QueryFrom(Db.Events.Table)
      .Join(Db.Users.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Where(Db.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId))
      .Where(Db.Users.EmailAddressNormalized, Operators.IsEqualTo(emailAddressNormalized))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return base.Load<UserAggregate>(events.Select(EventSerializer.Deserialize));
  }
  public async Task<IEnumerable<UserAggregate>> LoadAsync(RoleAggregate role, CancellationToken cancellationToken)
  {
    string aggregateId = role.Id.Value;

    IQuery query = Sql.QueryFrom(Db.Events.Table)
      .Join(Db.Users.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Join(Db.UserRoles.UserId, Db.Users.UserId)
      .Join(Db.Roles.RoleId, Db.UserRoles.RoleId)
      .Where(Db.Roles.AggregateId, Operators.IsEqualTo(aggregateId))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return base.Load<UserAggregate>(events.Select(EventSerializer.Deserialize));
  }
}
