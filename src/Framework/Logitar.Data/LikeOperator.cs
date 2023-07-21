namespace Logitar.Data;

/// <summary>
/// Represents a LIKE data query operator.
/// </summary>
public record LikeOperator : ConditionalOperator
{
  /// <summary>
  /// Initializes a new instance of the <see cref="LikeOperator"/> class.
  /// </summary>
  /// <param name="pattern">The pattern used to match strings.</param>
  /// <param name="notLike">A value indicating whether or not the pattern will be used to exclude matching strings.</param>
  /// <exception cref="ArgumentException">The pattern was missing.</exception>
  public LikeOperator(string pattern, bool notLike = false)
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
