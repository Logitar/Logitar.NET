namespace Logitar.EventSourcing.Infrastructure;

public abstract class AggregateRepository : IAggregateRepository
{
  protected AggregateRepository(IEventBus eventBus)
  {
    EventBus = eventBus;
  }

  protected IEventBus EventBus { get; }

  public virtual async Task<T?> LoadAsync<T>(AggregateId id, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    return await LoadAsync<T>(id, includeDeleted: false, cancellationToken);
  }
  public virtual async Task<T?> LoadAsync<T>(AggregateId id, long? version, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    return await LoadAsync<T>(id, version, includeDeleted: false, cancellationToken);
  }
  public virtual async Task<T?> LoadAsync<T>(AggregateId id, bool includeDeleted, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    return await LoadAsync<T>(id, version: null, includeDeleted, cancellationToken);
  }
  public virtual async Task<T?> LoadAsync<T>(AggregateId id, long? version, bool includeDeleted, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    IEnumerable<DomainEvent> changes = await LoadChangesAsync<T>(id, version, includeDeleted, cancellationToken);
    return Load<T>(changes, includeDeleted).SingleOrDefault();
  }
  protected abstract Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(AggregateId id, long? version, bool includeDeleted,
    CancellationToken cancellationToken);

  public virtual async Task<IEnumerable<T>> LoadAsync<T>(CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    return await LoadAsync<T>(includeDeleted: false, cancellationToken);
  }
  public virtual async Task<IEnumerable<T>> LoadAsync<T>(bool includeDeleted, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    IEnumerable<DomainEvent> changes = await LoadChangesAsync<T>(includeDeleted, cancellationToken);
    return Load<T>(changes, includeDeleted);
  }
  protected abstract Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(bool includeDeleted, CancellationToken cancellationToken);

  public virtual async Task<IEnumerable<T>> LoadAsync<T>(IEnumerable<AggregateId> ids, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    return await LoadAsync<T>(ids, includeDeleted: false, cancellationToken);
  }
  public virtual async Task<IEnumerable<T>> LoadAsync<T>(IEnumerable<AggregateId> ids, bool includeDeleted, CancellationToken cancellationToken)
    where T : AggregateRoot
  {
    IEnumerable<DomainEvent> changes = await LoadChangesAsync<T>(ids, includeDeleted, cancellationToken);
    return Load<T>(changes, includeDeleted);
  }
  protected abstract Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(IEnumerable<AggregateId> ids, bool includeDeleted, CancellationToken cancellationToken);

  public virtual async Task SaveAsync(AggregateRoot aggregate, CancellationToken cancellationToken)
  {
    await SaveAsync(new[] { aggregate }, cancellationToken);
  }
  public virtual async Task SaveAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken)
  {
    await SaveChangesAsync(aggregates, cancellationToken);

    await PublishAndClearChangesAsync(aggregates, cancellationToken);
  }
  protected abstract Task SaveChangesAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken);

  protected virtual IEnumerable<T> Load<T>(IEnumerable<DomainEvent> events, bool includeDeleted = false)
    where T : AggregateRoot
  {
    List<T> aggregates = new(events.Count());

    IEnumerable<IGrouping<AggregateId, DomainEvent>> groups = events.GroupBy(e => e.AggregateId);
    foreach (IGrouping<AggregateId, DomainEvent> group in groups)
    {
      T aggregate = AggregateRoot.LoadFromChanges<T>(group.Key, group);
      if (!aggregate.IsDeleted || includeDeleted)
      {
        aggregates.Add(aggregate);
      }
    }

    return aggregates.AsReadOnly();
  }

  protected async Task PublishAndClearChangesAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken)
  {
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
