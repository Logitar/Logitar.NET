namespace Logitar.Data.PostgreSQL;

/// <summary>
/// Represents a ILIKE data query operator.
/// </summary>
public record InsensitiveLikeOperator : ConditionalOperator
{
  /// <summary>
  /// Initializes a new instance of the <see cref="InsensitiveLikeOperator"/> class.
  /// </summary>
  /// <param name="pattern">The pattern used to match strings.</param>
  /// <param name="notLike">A value indicating whether or not the pattern will be used to exclude matching strings.</param>
  /// <exception cref="ArgumentException">The pattern was missing.</exception>
  public InsensitiveLikeOperator(string pattern, bool notLike = false)
  {
    if (string.IsNullOrWhiteSpace(pattern))
    {
      throw new ArgumentException("The pattern is required.", nameof(pattern));
    }

    Pattern = pattern;
    NotLike = notLike;
  }

  /// <summary>
  /// Gets the pattern used to match strings.
  /// </summary>
  public string Pattern { get; }
  /// <summary>
  /// Gets a value indicating whether or not the pattern will be used to exclude matching strings.
  /// </summary>
  public bool NotLike { get; }
}
