namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class ConditionGroupTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct ConditionGroup.")]
  [InlineData("test")]
  [InlineData("  test  ")]
  public void Ctor_it_constructs_the_correct_ConditionGroup(string @operator)
  {
    Condition[] conditions = new[]
    {
      new OperatorCondition(new ColumnId("Id"), new NullOperator(notNull: true))
    };
    ConditionGroupMock conditionGroup = new(conditions, @operator);
    Assert.Same(conditions, conditionGroup.Conditions);
    Assert.Equal(@operator.Trim(), conditionGroup.Operator);
  }

  [Fact(DisplayName = "Ctor: it throws ArgumentException when conditions are empty.")]
  public void Ctor_it_throws_ArgumentException_when_conditions_are_empty()
  {
    var exception = Assert.Throws<ArgumentException>(() => new ConditionGroupMock(Array.Empty<Condition>(), "AND"));
    Assert.Equal("conditions", exception.ParamName);
  }

  [Theory(DisplayName = "Ctor: it throws ArgumentException when operator is null, empty, or only white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void Ctor_it_throws_ArgumentException_when_operator_is_null_empty_or_only_white_space(string? @operator)
  {
    Condition[] conditions = new[]
    {
      new OperatorCondition(new ColumnId("Id"), new NullOperator(notNull: true))
    };
    var exception = Assert.Throws<ArgumentException>(() => new ConditionGroupMock(conditions, @operator!));
  }
}
