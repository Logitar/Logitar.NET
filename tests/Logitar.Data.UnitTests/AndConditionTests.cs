namespace Logitar.Data.UnitTests;

[Trait(Traits.Category, Categories.Unit)]
public class AndConditionTests
{
  [Fact(DisplayName = "Ctor: it constructs the correct AndCondition.")]
  public void Ctor_it_constructs_the_correct_AndCondition()
  {
    Condition[] conditions = new[]
    {
      new OperatorCondition(new ColumnId("Id"), new NullOperator(notNull: true))
    };
    AndCondition andCondition = new(conditions);
    Assert.Same(conditions, andCondition.Conditions);
    Assert.Equal("AND", andCondition.Operator);
  }
}
