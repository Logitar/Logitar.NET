namespace Logitar.Data;

/// <summary>
/// Represents a group of conditions.
/// </summary>
public abstract record ConditionGroup : Condition
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ConditionGroup"/> class.
  /// </summary>
  /// <param name="conditions">The list of underlying conditions.</param>
  /// <param name="operator">The logical operator to use between the underlying conditions.</param>
  /// <exception cref="ArgumentException">No underlying condition has been specified, or the logical operator was missing.</exception>
  protected ConditionGroup(IEnumerable<Condition> conditions, string @operator)
  {
    if (!conditions.Any())
    {
      throw new ArgumentException("At least one condition must be specified.", nameof(conditions));
    }
    if (string.IsNullOrWhiteSpace(@operator))
    {
      throw new ArgumentException("The condition group operator is required.", nameof(@operator));
    }

    Conditions = conditions;
    Operator = @operator.Trim();
  }

  /// <summary>
  /// Gets the list of underlying conditions.
  /// </summary>
  public IEnumerable<Condition> Conditions { get; }
  /// <summary>
  /// Gets the logical operator to use between the underlying conditions.
  /// </summary>
  public string Operator { get; }
}
