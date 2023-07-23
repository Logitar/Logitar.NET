using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

public class UserRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IUserRepository
{
  private static readonly string _aggregateType = typeof(UserAggregate).GetName();

  public UserRepository(ICurrentActor currentActor, IEventBus eventBus, EventContext eventContext)
    : base(eventBus, eventContext)
  {
    CurrentActor = currentActor;
  }

  protected ICurrentActor CurrentActor { get; }

  public async Task<UserAggregate?> LoadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string uniqueNameNormalized = uniqueName.Trim().ToUpper();

    IQuery query = SqlServerQueryBuilder.From(Db.Events.Table)
      .Join(new Join(Db.Users.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(_aggregateType)))
      )
      .WhereAnd(
        new OperatorCondition(Db.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId)),
        new OperatorCondition(Db.Users.UniqueNameNormalized, Operators.IsEqualTo(uniqueNameNormalized))
      )
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .OrderBy(e => e.Version)
      .AsNoTracking()
      .ToArrayAsync(cancellationToken);

    return Load<UserAggregate>(events.Select(EventSerializer.Instance.Deserialize)).SingleOrDefault();
  }
  public async Task<IEnumerable<UserAggregate>> LoadAsync(string? tenantId, IEmailAddress email, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string emailAddressNormalized = email.Address.ToUpper();

    IQuery query = SqlServerQueryBuilder.From(Db.Events.Table)
      .Join(new Join(Db.Users.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(_aggregateType)))
      )
      .WhereAnd(
        new OperatorCondition(Db.Users.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId)),
        new OperatorCondition(Db.Users.EmailAddressNormalized, Operators.IsEqualTo(emailAddressNormalized))
      )
      .SelectAll(Db.Events.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .OrderBy(e => e.Version)
      .AsNoTracking()
      .ToArrayAsync(cancellationToken);

    return Load<UserAggregate>(events.Select(EventSerializer.Instance.Deserialize));
  }

  public async Task SaveAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    AssignActor(user);

    await base.SaveAsync(user, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<UserAggregate> users, CancellationToken cancellationToken)
  {
    AssignActor(users);

    await base.SaveAsync(users, cancellationToken);
  }

  private void AssignActor(AggregateRoot aggregate) => AssignActor(new[] { aggregate });
  private void AssignActor(IEnumerable<AggregateRoot> aggregates)
  {
    foreach (AggregateRoot aggregate in aggregates)
    {
      foreach (DomainEvent change in aggregate.Changes)
      {
        change.ActorId ??= CurrentActor.Actor.Id;
      }
    }
  }
}
