using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.EventSourcing.Infrastructure;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Domain.Sessions;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.SqlServer.Repositories;

public class SessionRepository : EventSourcing.EntityFrameworkCore.Relational.AggregateRepository, ISessionRepository
{
  private static readonly string _aggregateType = typeof(SessionAggregate).GetName();

  public SessionRepository(ICurrentActor currentActor, IEventBus eventBus, EventContext eventContext)
    : base(eventBus, eventContext)
  {
    CurrentActor = currentActor;
  }

  protected ICurrentActor CurrentActor { get; }

  public async Task<SessionAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken)
    => await base.LoadAsync<SessionAggregate>(id, cancellationToken);

  public async Task SaveAsync(SessionAggregate session, CancellationToken cancellationToken)
  {
    AssignActor(session);

    await base.SaveAsync(session, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<SessionAggregate> sessions, CancellationToken cancellationToken)
  {
    AssignActor(sessions);

    await base.SaveAsync(sessions, cancellationToken);
  }

  #region TODO(fpion): refactor
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
  #endregion
}
