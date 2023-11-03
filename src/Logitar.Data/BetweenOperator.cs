namespace Logitar.Data;

/// <summary>
/// Represents a BETWEEN data query operator.
/// </summary>
public record BetweenOperator : ConditionalOperator
{
  /// <summary>
  /// Initializes a new instance of the <see cref="BetweenOperator"/> class.
  /// </summary>
  /// <param name="minValue">The minimum value of the range.</param>
  /// <param name="maxValue">The maximum value of the range.</param>
  /// <param name="notBetween">A value indicating whether or not the range is an excluding range.</param>
  public BetweenOperator(object minValue, object maxValue, bool notBetween = false)
  {
    MinValue = minValue;
    MaxValue = maxValue;
    NotBetween = notBetween;
  }

  /// <summary>
  /// Gets the minimum value of the range.
  /// </summary>
  public object MinValue { get; }
  /// <summary>
  /// Gets the maximum value of the range.
  /// </summary>
  public object MaxValue { get; }
  /// <summary>
  /// Gets a value indicating whether or not the range is an excluding range.
  /// </summary>
  public bool NotBetween { get; }
}
