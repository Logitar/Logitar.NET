namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record AndCondition : ConditionGroup
{
  public AndCondition(params Condition[] conditions) : base(conditions, "AND")
  {
  }
}
