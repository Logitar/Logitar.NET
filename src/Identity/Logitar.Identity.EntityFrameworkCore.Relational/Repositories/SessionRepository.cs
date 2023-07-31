using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Constants;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class SessionRepository : IdentityRepository, ISessionRepository
{
  private static readonly string _aggregateType = typeof(SessionAggregate).GetName();

  public SessionRepository(ICurrentActor currentActor, IEventBus eventBus, EventContext eventContext,
    IQueryHelper queryHelper) : base(currentActor, eventBus, eventContext, queryHelper)
  {
  }

  public async Task<IEnumerable<SessionAggregate>> LoadAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    IQuery query = QueryHelper.From(Db.Events.Table)
      .Join(Db.Sessions.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(_aggregateType))
      )
      .Join(Db.Users.UserId, Db.Sessions.UserId,
        new OperatorCondition(Db.Users.AggregateId, Operators.IsEqualTo(user.Id.Value))
      )
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .OrderBy(e => e.Version)
      .AsNoTracking()
      .ToArrayAsync(cancellationToken);

    return Load<SessionAggregate>(events.Select(EventSerializer.Instance.Deserialize));
  }

  public async Task<IEnumerable<SessionAggregate>> LoadActiveAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    IQuery query = QueryHelper.From(Db.Events.Table)
      .Join(Db.Sessions.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(_aggregateType))
      )
      .Join(Db.Users.UserId, Db.Sessions.UserId,
        new OperatorCondition(Db.Users.AggregateId, Operators.IsEqualTo(user.Id.Value))
      )
      .Where(Db.Sessions.IsActive, Operators.IsEqualTo(true))
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .OrderBy(e => e.Version)
      .AsNoTracking()
      .ToArrayAsync(cancellationToken);

    return Load<SessionAggregate>(events.Select(EventSerializer.Instance.Deserialize));
  }

  public async Task<SessionAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken)
    => await base.LoadAsync<SessionAggregate>(id, cancellationToken);

  public async Task SaveAsync(SessionAggregate session, CancellationToken cancellationToken)
  {
    await base.SaveAsync(session, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<SessionAggregate> sessions, CancellationToken cancellationToken)
  {
    await base.SaveAsync(sessions, cancellationToken);
  }
}
