using Logitar.EventSourcing.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Logitar.EventSourcing.EntityFrameworkCore.Relational;

public class RelationalAggregateRepository : AggregateRepository
{
  public RelationalAggregateRepository(IEventBus eventBus, EventContext eventContext) : base(eventBus)
  {
    EventContext = eventContext;
  }

  protected EventContext EventContext { get; }

  public override async Task<T?> LoadAsync<T>(AggregateId id, long? version, bool includeDeleted, CancellationToken cancellationToken)
  {
    string aggregateId = id.Value;
    string aggregateType = typeof(T).GetName();

    EventEntity[] events = await EventContext.Events.AsNoTracking()
      .Where(e => e.AggregateType == aggregateType && e.AggregateId == aggregateId)
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<T>(events, includeDeleted).SingleOrDefault();
  }

  public override async Task<IEnumerable<T>> LoadAsync<T>(bool includeDeleted, CancellationToken cancellationToken)
  {
    string aggregateType = typeof(T).GetName();

    EventEntity[] events = await EventContext.Events.AsNoTracking()
      .Where(e => e.AggregateType == aggregateType)
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<T>(events, includeDeleted);
  }

  public override async Task<IEnumerable<T>> LoadAsync<T>(IEnumerable<AggregateId> ids, bool includeDeleted, CancellationToken cancellationToken)
  {
    HashSet<string> aggregateIds = ids.Select(id => id.Value).ToHashSet();
    string aggregateType = typeof(T).GetName();

    EventEntity[] events = await EventContext.Events.AsNoTracking()
      .Where(e => e.AggregateType == aggregateType && aggregateIds.Contains(e.AggregateId))
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<T>(events, includeDeleted);
  }

  public override async Task SaveAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken)
  {
    IEnumerable<EventEntity> events = aggregates.SelectMany(EventEntity.FromChanges);

    EventContext.Events.AddRange(events);
    await EventContext.SaveChangesAsync(cancellationToken);

    await base.PublishAndClearChangesAsync(aggregates, cancellationToken);
  }
}
