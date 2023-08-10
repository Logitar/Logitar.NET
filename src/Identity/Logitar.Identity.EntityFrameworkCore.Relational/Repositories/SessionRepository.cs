using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class SessionRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, ISessionRepository
{
  public SessionRepository(IEventBus eventBus, EventContext eventContext,
    IEventSerializer eventSerializer, ISqlHelper sqlHelper)
      : base(eventBus, eventContext, eventSerializer)
  {
    Sql = sqlHelper;
  }

  protected string AggregateType { get; } = typeof(SessionAggregate).GetName();
  protected ISqlHelper Sql { get; private set; }

  public async Task<SessionAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken)
    => await base.LoadAsync<SessionAggregate>(id, cancellationToken);
  public async Task<SessionAggregate?> LoadAsync(AggregateId id, long? version, CancellationToken cancellationToken)
    => await base.LoadAsync<SessionAggregate>(id, version, cancellationToken);
  public async Task<SessionAggregate?> LoadAsync(AggregateId id, bool includeDeleted, CancellationToken cancellationToken)
    => await base.LoadAsync<SessionAggregate>(id, includeDeleted, cancellationToken);
  public async Task<SessionAggregate?> LoadAsync(AggregateId id, long? version, bool includeDeleted, CancellationToken cancellationToken)
    => await base.LoadAsync<SessionAggregate>(id, version, includeDeleted, cancellationToken);

  public async Task<IEnumerable<SessionAggregate>> LoadAsync(CancellationToken cancellationToken)
    => await base.LoadAsync<SessionAggregate>(cancellationToken);
  public async Task<IEnumerable<SessionAggregate>> LoadAsync(bool includeDeleted, CancellationToken cancellationToken)
    => await base.LoadAsync<SessionAggregate>(includeDeleted, cancellationToken);

  public async Task<IEnumerable<SessionAggregate>> LoadAsync(IEnumerable<AggregateId> ids, CancellationToken cancellationToken)
    => await base.LoadAsync<SessionAggregate>(ids, cancellationToken);
  public async Task<IEnumerable<SessionAggregate>> LoadAsync(IEnumerable<AggregateId> ids, bool includeDeleted, CancellationToken cancellationToken)
    => await base.LoadAsync<SessionAggregate>(ids, includeDeleted, cancellationToken);

  public async Task<IEnumerable<SessionAggregate>> LoadAsync(string? tenantId, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();

    IQuery query = Sql.QueryFrom(Db.Events.Table)
      .Join(Db.Sessions.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Join(Db.Users.UserId, Db.Sessions.UserId)
      .Where(Db.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return base.Load<SessionAggregate>(events.Select(EventSerializer.Deserialize));
  }
  public async Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken)
    => await LoadAsync(user, isActive: null, cancellationToken);
  public async Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, bool? isActive, CancellationToken cancellationToken)
  {
    string aggregateId = user.Id.Value;

    IQueryBuilder builder = Sql.QueryFrom(Db.Events.Table)
      .Join(Db.Sessions.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(AggregateType))
      )
      .Join(Db.Users.UserId, Db.Sessions.UserId)
      .Where(Db.Users.AggregateId, Operators.IsEqualTo(aggregateId));

    if (isActive.HasValue)
    {
      builder = builder.Where(Db.Sessions.IsActive, Operators.IsEqualTo(isActive.Value));
    }

    IQuery query = builder.SelectAll(Db.Events.Table).Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .AsNoTracking()
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return base.Load<SessionAggregate>(events.Select(EventSerializer.Deserialize));
  }

  public async Task SaveAsync(SessionAggregate role, CancellationToken cancellationToken)
    => await base.SaveAsync(role, cancellationToken);
  public async Task SaveAsync(IEnumerable<SessionAggregate> roles, CancellationToken cancellationToken)
    => await base.SaveAsync(roles, cancellationToken);
}
