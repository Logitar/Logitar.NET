namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class OrConditionTests
{
  [Fact(DisplayName = "Ctor: it constructs the correct OrCondition.")]
  public void Ctor_it_constructs_the_correct_OrCondition()
  {
    Condition[] conditions = new[]
    {
      new OperatorCondition(new ColumnId("Id"), new NullOperator(notNull: true))
    };
    OrCondition orCondition = new(conditions);
    Assert.Same(conditions, orCondition.Conditions);
    Assert.Equal("OR", orCondition.Operator);
  }
}
