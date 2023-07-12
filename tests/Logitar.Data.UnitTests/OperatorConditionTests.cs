namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class OperatorConditionTests
{
  [Fact(DisplayName = "Ctor: it constructs the correct OperatorCondition.")]
  public void Ctor_it_constructs_the_correct_OperatorCondition()
  {
    ColumnId column = new("Id");
    NullOperator @operator = new(notNull: true);
    OperatorCondition condition = new(column, @operator);
    Assert.Equal(column, condition.Column);
    Assert.Equal(@operator, condition.Operator);
  }

  [Fact(DisplayName = "Ctor: it throws ArgumentException when column name is null.")]
  public void Ctor_it_throws_ArgumentException_when_column_name_is_null()
  {
    var exception = Assert.Throws<ArgumentException>(() => new OperatorCondition(ColumnId.All(), new NullOperator()));
    Assert.Equal("column", exception.ParamName);
  }
}
