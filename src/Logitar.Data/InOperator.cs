namespace Logitar.Data;

/// <summary>
/// Represents an IN data query operator.
/// </summary>
public record InOperator : ConditionalOperator
{
  /// <summary>
  /// Initializes a new instance of the <see cref="InOperator"/> class.
  /// </summary>
  /// <param name="values">The values in the range.</param>
  public InOperator(params object[] values) : this(notIn: false, values)
  {
  }
  /// <summary>
  /// Initializes a new instance of the <see cref="InOperator"/> class.
  /// </summary>
  /// <param name="notIn">A value indicating whether or not the range is an excluding range.</param>
  /// <param name="values">The values in the range.</param>
  /// <exception cref="ArgumentException">No value was specified for the range.</exception>
  public InOperator(bool notIn, params object[] values)
  {
    if (!values.Any())
    {
      throw new ArgumentException("At least one value must be specified.", nameof(values));
    }

    Values = values;
    NotIn = notIn;
  }

  /// <summary>
  /// Gets the values in the range.
  /// </summary>
  public IEnumerable<object> Values { get; }
  /// <summary>
  /// Gets a value indicating whether or not the range is an excluding range.
  /// </summary>
  public bool NotIn { get; }
}
