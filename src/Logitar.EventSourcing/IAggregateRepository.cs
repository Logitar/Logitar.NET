namespace Logitar.EventSourcing;

/// <summary>
/// TODO(fpion): document
/// </summary>
public interface IAggregateRepository
{
  Task<T?> LoadAsync<T>(AggregateId id, CancellationToken cancellationToken = default) where T : AggregateRoot;
  Task<T?> LoadAsync<T>(AggregateId id, long? version, CancellationToken cancellationToken = default) where T : AggregateRoot;
  Task<T?> LoadAsync<T>(AggregateId id, bool includeDeleted, CancellationToken cancellationToken = default) where T : AggregateRoot;
  Task<T?> LoadAsync<T>(AggregateId id, long? version, bool includeDeleted, CancellationToken cancellationToken = default) where T : AggregateRoot;

  Task<IEnumerable<T>> LoadAsync<T>(CancellationToken cancellationToken = default) where T : AggregateRoot;
  Task<IEnumerable<T>> LoadAsync<T>(bool includeDeleted, CancellationToken cancellationToken = default) where T : AggregateRoot;

  Task<IEnumerable<T>> LoadAsync<T>(IEnumerable<AggregateId> ids, CancellationToken cancellationToken = default) where T : AggregateRoot;
  Task<IEnumerable<T>> LoadAsync<T>(IEnumerable<AggregateId> ids, bool includeDeleted, CancellationToken cancellationToken = default) where T : AggregateRoot;

  Task SaveAsync(AggregateRoot aggregate, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken = default);
}
