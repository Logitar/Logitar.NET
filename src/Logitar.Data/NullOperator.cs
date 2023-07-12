namespace Logitar.Data;

/// <summary>
/// Represents a NULL data query operator.
/// </summary>
public record NullOperator : ConditionalOperator
{
  /// <summary>
  /// Initializes a new instance of the <see cref="NullOperator"/> class.
  /// </summary>
  /// <param name="notNull">A value indicating whether or not null results will be excluded.</param>
  public NullOperator(bool notNull = false)
  {
    NotNull = notNull;
  }

  /// <summary>
  /// Gets a value indicating whether or not null results will be excluded.
  /// </summary>
  public bool NotNull { get; }
}
