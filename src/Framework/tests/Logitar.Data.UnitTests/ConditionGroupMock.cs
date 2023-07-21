namespace Logitar.Data;

internal record ConditionGroupMock : ConditionGroup
{
  public ConditionGroupMock(IEnumerable<Condition> conditions, string @operator) : base(conditions, @operator)
  {
  }
}
