namespace Logitar.Data;

/// <summary>
/// TODO(fpion): document
/// </summary>
public abstract record ConditionGroup : Condition
{
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

  public IEnumerable<Condition> Conditions { get; }
  public string Operator { get; }
}
