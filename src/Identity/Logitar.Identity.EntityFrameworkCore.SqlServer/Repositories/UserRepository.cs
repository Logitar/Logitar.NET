using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Users;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

public class UserRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, IUserRepository
{
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

    //IQuery query = SqlServerQueryBuilder.From(Db.Event.Table)
    //  // JOIN "Users" u ON u."AggregateId" = e."AggregateId" AND e."AggregateType" = @AggregateType
    //  .WhereAnd(
    //    new OperatorCondition(Db.User.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId)),
    //    new OperatorCondition(Db.User.UniqueNameNormalized, Operators.IsEqualTo(uniqueNameNormalized))
    //  )
    //  .SelectAll(Db.Event.Table)
    //  .Build(); // TODO(fpion): implement JOIN

    string sql = "SELECT [dbo].[Events].* FROM [dbo].[Events] JOIN [dbo].[Users] ON [dbo].[Users].[AggregateId] = [dbo].[Events].[AggregateId] AND [dbo].[Events].[AggregateType] = @AggregateType WHERE ([dbo].[Users].[TenantId] = @TenantId AND [dbo].[Users].[UniqueNameNormalized] = @UniqueNameNormalized)";
    object[] parameters = new[]
    {
      new SqlParameter("AggregateType", typeof(UserAggregate).GetName()),
      new SqlParameter("TenantId", tenantId),
      new SqlParameter("UniqueNameNormalized", uniqueNameNormalized)
    };
    EventEntity[] events = await EventContext.Events.FromSqlRaw(sql, parameters)
      .OrderBy(e => e.Version)
      .AsNoTracking()
      .ToArrayAsync(cancellationToken);

    return Load<UserAggregate>(events.Select(EventSerializer.Instance.Deserialize)).SingleOrDefault();
  }
  public async Task<IEnumerable<UserAggregate>> LoadAsync(string? tenantId, IEmailAddress email, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string emailAddressNormalized = email.Address.ToUpper();

    //IQuery query = SqlServerQueryBuilder.From(Db.Event.Table)
    //  // JOIN "Users" u ON u."AggregateId" = e."AggregateId" AND e."AggregateType" = @AggregateType
    //  .WhereAnd(
    //    new OperatorCondition(Db.User.TenantId, tenantId == null ? Operators.IsNull() : Operators.IsEqualTo(tenantId)),
    //    new OperatorCondition(Db.User.EmailAddressNormalized, Operators.IsEqualTo(emailAddressNormalized))
    //  )
    //  .SelectAll(Db.Event.Table)
    //  .Build(); // TODO(fpion): implement JOIN

    string sql = "SELECT [dbo].[Events].* FROM [dbo].[Events] JOIN [dbo].[Users] ON [dbo].[Users].[AggregateId] = [dbo].[Events].[AggregateId] AND [dbo].[Events].[AggregateType] = @AggregateType WHERE ([dbo].[Users].[TenantId] = @TenantId AND [dbo].[Users].[EmailAddressNormalized] = @EmailAddressNormalized)";
    object[] parameters = new[]
    {
      new SqlParameter("AggregateType", typeof(UserAggregate).GetName()),
      new SqlParameter("TenantId", tenantId),
      new SqlParameter("EmailAddressNormalized", emailAddressNormalized)
    };
    EventEntity[] events = await EventContext.Events.FromSqlRaw(sql, parameters)
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
