﻿using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Constants;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public class UserRepository : IdentityRepository, IUserRepository
{
  private static readonly string _aggregateType = typeof(UserAggregate).GetName();

  public UserRepository(ICurrentActor currentActor, IEventBus eventBus, EventContext eventContext,
    IQueryHelper queryHelper) : base(currentActor, eventBus, eventContext, queryHelper)
  {
  }

  public async Task<UserAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken)
    => await base.LoadAsync<UserAggregate>(id, cancellationToken);

  public async Task<UserAggregate?> LoadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken)
  {
    tenantId = tenantId?.CleanTrim();
    string uniqueNameNormalized = uniqueName.Trim().ToUpper();

    IQuery query = QueryHelper.From(Db.Events.Table)
      .Join(Db.Users.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(_aggregateType))
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

    IQuery query = QueryHelper.From(Db.Events.Table)
      .Join(Db.Users.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(_aggregateType))
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

  public async Task<IEnumerable<UserAggregate>> LoadAsync(RoleAggregate role, CancellationToken cancellationToken)
  {
    IQuery query = QueryHelper.From(Db.Events.Table)
      .Join(Db.Users.AggregateId, Db.Events.AggregateId,
        new OperatorCondition(Db.Events.AggregateType, Operators.IsEqualTo(_aggregateType))
      )
      .Join(Db.UserRoles.UserId, Db.Users.UserId)
      .Join(Db.Roles.RoleId, Db.UserRoles.RoleId)
      .Where(Db.Roles.AggregateId, Operators.IsEqualTo(role.Id.Value))
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
    await base.SaveAsync(user, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<UserAggregate> users, CancellationToken cancellationToken)
  {
    await base.SaveAsync(users, cancellationToken);
  }
}
