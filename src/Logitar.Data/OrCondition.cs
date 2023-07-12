namespace Logitar.Data;

/// <summary>
/// Represents an union of conditions. Only one of specified conditions must be true for the union to return true.
/// </summary>
public record OrCondition : ConditionGroup
{
  /// <summary>
  /// Initializes a new instance of the <see cref="OrCondition"/> class.
  /// </summary>
  /// <param name="conditions">The list of underlying conditions.</param>
  public OrCondition(params Condition[] conditions) : base(conditions, "OR")
  {
  }
}
