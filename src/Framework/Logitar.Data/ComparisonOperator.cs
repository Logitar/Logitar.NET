namespace Logitar.Data;

/// <summary>
/// Represents a comparison operator of a data query.
/// </summary>
public record ComparisonOperator : ConditionalOperator
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ComparisonOperator"/> class.
  /// </summary>
  /// <param name="operator">The comparison operator to use.</param>
  /// <param name="value">The value of the comparison.</param>
  /// <exception cref="ArgumentException">The comparison operator was missing.</exception>
  public ComparisonOperator(string @operator, object value)
  {
    if (string.IsNullOrWhiteSpace(@operator))
    {
      throw new ArgumentException("The comparison operator is required.", nameof(@operator));
    }

    Operator = @operator.Trim();
    Value = value;
  }

  /// <summary>
  /// Gets the comparison operator to use.
  /// </summary>
  public string Operator { get; }
  /// <summary>
  /// Gets the value of the comparison.
  /// </summary>
  public object Value { get; }
}
