namespace Logitar.EventSourcing;

/// <summary>
/// Represents a domain aggregate, which should be a tangible concept in a domain model.
/// </summary>
public abstract class AggregateRoot
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AggregateRoot"/> class.
  /// </summary>
  /// <param name="id">The identifier of the aggregate.</param>
  /// <exception cref="ArgumentException">The identifier value is missing.</exception>
  protected AggregateRoot(AggregateId? id = null)
  {
    id ??= AggregateId.NewId();
    if (string.IsNullOrWhiteSpace(id.Value.Value))
    {
      throw new ArgumentException("The identifier value is required.", nameof(id));
    }

    Id = id.Value;
  }

  /// <summary>
  /// Gets or sets the identifier of the aggregate.
  /// </summary>
  public AggregateId Id { get; private set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the aggregate is deleted.
  /// </summary>
  public bool IsDeleted { get; private set; }
  /// <summary>
  /// Gets or sets the version of the aggregate.
  /// </summary>
  public long Version { get; private set; }

  /// <summary>
  /// The uncommitted changes of the aggregate.
  /// </summary>
  private readonly List<DomainEvent> _changes = new();
  /// <summary>
  /// Gets a value indicating whether or not the aggregate has uncommitted changes.
  /// </summary>
  public bool HasChanges => _changes.Any();
  /// <summary>
  /// Gets the uncommitted changes of the aggregate.
  /// </summary>
  public IReadOnlyCollection<DomainEvent> Changes => _changes.AsReadOnly();
  /// <summary>
  /// Clears the uncommitted changes of the aggregate.
  /// </summary>
  public void ClearChanges() => _changes.Clear();

  /// <summary>
  /// Loads an aggregate from its changes and assign its identifier.
  /// </summary>
  /// <typeparam name="T">The type of the aggregate to load.</typeparam>
  /// <param name="id">The identifier of the aggregate.</param>
  /// <param name="changes">The changes of the aggregate.</param>
  /// <exception cref="AggregateConstructionFailedException">The aggregate construction failed.</exception>
  /// <exception cref="MissingAggregateConstructorException">The aggregate does not declare a public identifier constructor.</exception>
  /// <returns>The loaded aggregate.</returns>
  public static T LoadFromChanges<T>(AggregateId id, IEnumerable<DomainEvent> changes) where T : AggregateRoot
  {
    ConstructorInfo constructor = typeof(T).GetConstructor(new[] { typeof(AggregateId) })
      ?? throw new MissingAggregateConstructorException<T>();

    T aggregate = (T?)constructor.Invoke(new object[] { id })
      ?? throw new AggregateConstructionFailedException<T>(id);

    IOrderedEnumerable<DomainEvent> ordered = changes.OrderBy(e => e.Version);
    foreach (DomainEvent change in ordered)
    {
      aggregate.Dispatch(change);
    }

    return aggregate;
  }

  /// <summary>
  /// Applies the specified change to the current aggregate.
  /// </summary>
  /// <param name="change">The change to apply.</param>
  /// <param name="actorId">The identifier of the actor who triggered the event.</param>
  /// <param name="occurredOn">The date and time when the event occurred.</param>
  protected void ApplyChange(DomainEvent change, string? actorId = null, DateTime? occurredOn = null)
  {
    if (change.Id == default)
    {
      change.Id = Guid.NewGuid();
    }
    change.AggregateId ??= Id;
    if (change.Version == default)
    {
      change.Version = Version + 1;
    }
    change.ActorId ??= actorId;
    if (change.OccurredOn == default)
    {
      change.OccurredOn = occurredOn ?? DateTime.Now;
    }

    Dispatch(change);

    _changes.Add(change);
  }
  /// <summary>
  /// Dispatchs the specified event in the current aggregate, effectively applying the change.
  /// </summary>
  /// <param name="change">The event to dispatch.</param>
  /// <exception cref="CannotApplyPastEventException">The event is past the current aggregate's state.</exception>
  /// <exception cref="EventAggregateMismatchException">The event does not belong to the current aggregate.</exception>
  private void Dispatch(DomainEvent change)
  {
    if (change.AggregateId != Id)
    {
      throw new EventAggregateMismatchException(this, change);
    }

    if (change.Version < Version)
    {
      throw new CannotApplyPastEventException(this, change);
    }

    MethodInfo? apply = GetType().GetMethod("Apply", BindingFlags.Instance | BindingFlags.NonPublic, new[] { change.GetType() });
    apply?.Invoke(this, new[] { change });

    Version = change.Version;

    switch (change.DeleteAction)
    {
      case DeleteAction.Delete:
        IsDeleted = true;
        break;
      case DeleteAction.Undelete:
        IsDeleted = false;
        break;
    }
  }

  /// <summary>
  /// Returns a value indicating whether or not the specified object is equal to the aggregate.
  /// </summary>
  /// <param name="obj">The object to be compared to.</param>
  /// <returns>True if the object is equal to the aggregate.</returns>
  public override bool Equals(object? obj)
  {
    return obj is AggregateRoot aggregate && aggregate.GetType().Equals(GetType()) && aggregate.Id == Id;
  }
  /// <summary>
  /// Returns the hash code of the current aggregate.
  /// </summary>
  /// <returns>The hash code.</returns>
  public override int GetHashCode() => HashCode.Combine(GetType(), Id);
  /// <summary>
  /// Returns a string representation of the aggregate.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => $"{GetType()} ({Id})";
}
