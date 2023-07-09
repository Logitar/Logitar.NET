using Logitar.EventSourcing.Infrastructure;

namespace Logitar.EventSourcing.InMemory;

public class AggregateRepository : Infrastructure.AggregateRepository
{
  private readonly List<EventEntity> _events = new();

  public AggregateRepository(IEventBus eventBus) : base(eventBus)
  {
  }

  protected override Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(AggregateId id, long? version, CancellationToken cancellationToken)
  {
    string aggregateType = typeof(T).GetName();
    string aggregateId = id.Value;

    IEnumerable<DomainEvent> changes = _events.Where(e => e.AggregateType == aggregateType && e.AggregateId == aggregateId
        && (!version.HasValue || version.Value <= e.Version))
      .OrderBy(e => e.Version)
      .Select(EventSerializer.Instance.Deserialize);

    return Task.FromResult(changes);
  }

  protected override Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(CancellationToken cancellationToken)
  {
    string aggregateType = typeof(T).GetName();

    IEnumerable<DomainEvent> changes = _events.Where(e => e.AggregateType == aggregateType)
      .OrderBy(e => e.Version)
      .Select(EventSerializer.Instance.Deserialize);

    return Task.FromResult(changes);
  }

  protected override Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(IEnumerable<AggregateId> ids, CancellationToken cancellationToken)
  {
    string aggregateType = typeof(T).GetName();
    HashSet<string> aggregateIds = ids.Select(id => id.Value).ToHashSet();

    IEnumerable<DomainEvent> changes = _events.Where(e => e.AggregateType == aggregateType && aggregateIds.Contains(e.AggregateId))
      .OrderBy(e => e.Version)
      .Select(EventSerializer.Instance.Deserialize);

    return Task.FromResult(changes);
  }

  protected override Task SaveChangesAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken)
  {
    IEnumerable<EventEntity> events = aggregates.SelectMany(EventEntity.FromChanges);
    _events.AddRange(events);

    return Task.CompletedTask;
  }
}
