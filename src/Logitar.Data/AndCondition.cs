namespace Logitar.Data;

/// <summary>
/// Represents an intersection of conditions. Each specified conditions must be true for the intersection to return true.
/// </summary>
public record AndCondition : ConditionGroup
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AndCondition"/> class.
  /// </summary>
  /// <param name="conditions">The list of underlying conditions.</param>
  public AndCondition(params Condition[] conditions) : base(conditions, "AND")
  {
  }
}
