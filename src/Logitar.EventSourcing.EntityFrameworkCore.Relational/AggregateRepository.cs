using Microsoft.EntityFrameworkCore;

namespace Logitar.EventSourcing.EntityFrameworkCore.Relational;

/// <summary>
/// TODO(fpion): document
/// </summary>
public class AggregateRepository : IAggregateRepository
{
  public AggregateRepository(IEventBus eventBus, EventContext eventContext)
  {
    EventBus = eventBus;
    EventContext = eventContext;
  }

  protected IEventBus EventBus { get; }
  protected EventContext EventContext { get; }

  public async Task<T?> LoadAsync<T>(AggregateId id, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    return await LoadAsync<T>(id, includeDeleted: false, cancellationToken);
  }
  public async Task<T?> LoadAsync<T>(AggregateId id, long? version, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    return await LoadAsync<T>(id, version, includeDeleted: false, cancellationToken);
  }
  public async Task<T?> LoadAsync<T>(AggregateId id, bool includeDeleted, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    return await LoadAsync<T>(id, version: null, includeDeleted, cancellationToken);
  }
  public async Task<T?> LoadAsync<T>(AggregateId id, long? version, bool includeDeleted, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    string aggregateId = id.Value;
    string aggregateType = typeof(T).GetName();

    EventEntity[] events = await EventContext.Events.AsNoTracking()
      .Where(e => e.AggregateType == aggregateType && e.AggregateId == aggregateId)
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<T>(events, includeDeleted).SingleOrDefault();
  }

  public async Task<IEnumerable<T>> LoadAsync<T>(CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    return await LoadAsync<T>(includeDeleted: false, cancellationToken);
  }
  public async Task<IEnumerable<T>> LoadAsync<T>(bool includeDeleted, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    string aggregateType = typeof(T).GetName();

    EventEntity[] events = await EventContext.Events.AsNoTracking()
      .Where(e => e.AggregateType == aggregateType)
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<T>(events, includeDeleted);
  }

  public async Task<IEnumerable<T>> LoadAsync<T>(IEnumerable<AggregateId> ids, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    return await LoadAsync<T>(ids, includeDeleted: false, cancellationToken);
  }
  public async Task<IEnumerable<T>> LoadAsync<T>(IEnumerable<AggregateId> ids, bool includeDeleted, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    HashSet<string> aggregateIds = ids.Select(id => id.Value).ToHashSet();
    string aggregateType = typeof(T).GetName();

    EventEntity[] events = await EventContext.Events.AsNoTracking()
      .Where(e => e.AggregateType == aggregateType && aggregateIds.Contains(e.AggregateId))
      .OrderBy(e => e.Version)
      .ToArrayAsync(cancellationToken);

    return Load<T>(events, includeDeleted);
  }

  private static IEnumerable<T> Load<T>(IEnumerable<EventEntity> events, bool includeDeleted = false)
    where T : AggregateRoot
  {
    List<T> aggregates = new(events.Count());

    IEnumerable<IGrouping<string, EventEntity>> groups = events.GroupBy(e => e.AggregateId);
    foreach (IGrouping<string, EventEntity> group in groups)
    {
      AggregateId id = new(group.Key);
      IEnumerable<DomainEvent> changes = group.Select(EventSerializer.Instance.Deserialize);

      T aggregate = AggregateRoot.LoadFromChanges<T>(id, changes);
      if (!aggregate.IsDeleted || includeDeleted)
      {
        aggregates.Add(aggregate);
      }
    }

    return aggregates.AsReadOnly();
  }

  public async Task SaveAsync(AggregateRoot aggregate, CancellationToken cancellationToken)
  {
    await SaveAsync(new[] { aggregate }, cancellationToken);
  }
  public async Task SaveAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken)
  {
    IEnumerable<EventEntity> events = aggregates.SelectMany(EventEntity.FromChanges);

    EventContext.Events.AddRange(events);
    await EventContext.SaveChangesAsync(cancellationToken);

    foreach (AggregateRoot aggregate in aggregates)
    {
      if (aggregate.HasChanges)
      {
        foreach (DomainEvent change in aggregate.Changes)
        {
          await EventBus.PublishAsync(change, cancellationToken);
        }

        aggregate.ClearChanges();
      }
    }
  }
}
