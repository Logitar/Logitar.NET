using Logitar.Data;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Constants;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

public class UserRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IUserRepository
{
  public UserRepository(IEventBus eventBus, EventContext eventContext)
    : base(eventBus, eventContext)
  {
  }

  public async Task<UserAggregate?> LoadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string uniqueNameNormalized = uniqueName.Trim().ToUpper();

    IQuery query = SqlServerQueryBuilder.From(Db.Event.Table)
      // JOIN "Users" u ON u."AggregateId" = e."AggregateId" AND e."AggregateType" = @AggregateType // TODO(fpion): implement
      .WhereAnd(
        new OperatorCondition(Db.User.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId)),
        new OperatorCondition(Db.User.UniqueNameNormalized, Operators.IsEqualTo(uniqueNameNormalized))
      )
      .SelectAll(Db.Event.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .OrderBy(e => e.Version)
      .AsNoTracking()
      .ToArrayAsync(cancellationToken);

    return Load<UserAggregate>(events.Select(EventSerializer.Instance.Deserialize)).Single();
  }
  public async Task<IEnumerable<UserAggregate>> LoadAsync(string? tenantId, IEmailAddress email, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string emailAddressNormalized = email.Address.ToUpper();

    IQuery query = SqlServerQueryBuilder.From(Db.Event.Table)
      // JOIN "Users" u ON u."AggregateId" = e."AggregateId" AND e."AggregateType" = @AggregateType // TODO(fpion): implement
      .WhereAnd(
        new OperatorCondition(Db.User.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId)),
        new OperatorCondition(Db.User.EmailAddressNormalized, Operators.IsEqualTo(emailAddressNormalized))
      )
      .SelectAll(Db.Event.Table)
      .Build();

    EventEntity[] events = await EventContext.Events.FromQuery(query)
      .OrderBy(e => e.Version)
      .AsNoTracking()
      .ToArrayAsync(cancellationToken);

    return Load<UserAggregate>(events.Select(EventSerializer.Instance.Deserialize));
  }

  public async Task SaveAsync(UserAggregate user, CancellationToken cancellationToken)
  {
    // TODO(fpion): assign ActorId

    await base.SaveAsync(user, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<UserAggregate> users, CancellationToken cancellationToken)
  {
    // TODO(fpion): assign ActorId

    await base.SaveAsync(users, cancellationToken);
  }
}
