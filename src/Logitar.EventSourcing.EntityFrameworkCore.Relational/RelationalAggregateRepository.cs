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

  protected override async Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(AggregateId id, long? version, bool includeDeleted,
    CancellationToken cancellationToken)
  {
    string aggregateId = id.Value;
    string aggregateType = typeof(T).GetName();

    EventEntity[] events = await EventContext.Events.AsNoTracking()
      .Where(e => e.AggregateType == aggregateType && e.AggregateId == aggregateId)
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return events.Select(EventSerializer.Instance.Deserialize);
  }

  protected override async Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(bool includeDeleted, CancellationToken cancellationToken)
  {
    string aggregateType = typeof(T).GetName();

    EventEntity[] events = await EventContext.Events.AsNoTracking()
      .Where(e => e.AggregateType == aggregateType)
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return events.Select(EventSerializer.Instance.Deserialize);
  }

  protected override async Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(IEnumerable<AggregateId> ids, bool includeDeleted,
    CancellationToken cancellationToken)
  {
    string aggregateType = typeof(T).GetName();
    HashSet<string> aggregateIds = ids.Select(id => id.Value).ToHashSet();

    EventEntity[] events = await EventContext.Events.AsNoTracking()
      .Where(e => e.AggregateType == aggregateType && aggregateIds.Contains(e.AggregateId))
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return events.Select(EventSerializer.Instance.Deserialize);
  }

  protected override async Task SaveChangesAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken)
  {
    IEnumerable<EventEntity> events = aggregates.SelectMany(EventEntity.FromChanges);

    EventContext.Events.AddRange(events);
    await EventContext.SaveChangesAsync(cancellationToken);
  }
}
