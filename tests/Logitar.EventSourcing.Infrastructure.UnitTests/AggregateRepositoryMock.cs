namespace Logitar.EventSourcing.Infrastructure;

internal class AggregateRepositoryMock : AggregateRepository
{
  public AggregateRepositoryMock(IEventBus eventBus) : base(eventBus)
  {
  }

  public ICollection<DomainEvent> Events { get; } = new List<DomainEvent>();

  protected override Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(AggregateId id, long? version, CancellationToken cancellationToken)
  {
    return Task.FromResult(Events.Where(e => e.AggregateId == id && (!version.HasValue || e.Version <= version.Value)));
  }

  protected override Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(CancellationToken cancellationToken)
  {
    return Task.FromResult(Events.AsEnumerable());
  }

  protected override Task<IEnumerable<DomainEvent>> LoadChangesAsync<T>(IEnumerable<AggregateId> ids, CancellationToken cancellationToken)
  {
    return Task.FromResult(Events.Where(e => ids.Contains(e.AggregateId)));
  }

  protected override Task SaveChangesAsync(IEnumerable<AggregateRoot> aggregates, CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
