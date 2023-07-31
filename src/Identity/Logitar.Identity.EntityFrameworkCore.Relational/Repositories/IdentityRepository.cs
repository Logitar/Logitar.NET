﻿using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Repositories;

public abstract class IdentityRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository
{
  protected IdentityRepository(ICurrentActor currentActor, IEventBus eventBus,
    EventContext eventContext, IQueryHelper queryHelper) : base(eventBus, eventContext)
  {
    CurrentActor = currentActor;
    QueryHelper = queryHelper;
  }

  protected ICurrentActor CurrentActor { get; }
  protected IQueryHelper QueryHelper { get; }

  public override async Task SaveAsync(AggregateRoot aggregate, CancellationToken cancellationToken)
  {
    AssignActor(aggregate);

    await base.SaveAsync(aggregate, cancellationToken);
  }

  public override async Task SaveAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken)
  {
    AssignActor(aggregates);

    await base.SaveAsync(aggregates, cancellationToken);
  }

  protected void AssignActor(AggregateRoot aggregate) => AssignActor(new[] { aggregate });
  protected void AssignActor(IEnumerable<AggregateRoot> aggregates)
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
