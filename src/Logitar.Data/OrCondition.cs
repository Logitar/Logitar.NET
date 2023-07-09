namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record OrCondition : ConditionGroup
{
  public OrCondition(params Condition[] conditions) : base(conditions, "OR")
  {
  }
}
