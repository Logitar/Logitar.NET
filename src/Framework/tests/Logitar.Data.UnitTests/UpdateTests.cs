namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class UpdateTests
{
  [Fact(DisplayName = "It should construct the correct Update.")]
  public void It_should_construct_the_correct_Update()
  {
    ColumnId column = new("OccurredOn");
    DateTime moment = DateTime.Now;
    Update update = new(column, moment);
    Assert.Equal(column, update.Column);
    Assert.Equal(moment, update.Value);
  }

  [Fact(DisplayName = "It should throw ArgumentException when column name is null.")]
  public void It_should_throw_ArgumentException_when_column_name_is_null()
  {
    var exception = Assert.Throws<ArgumentException>(() => new Update(ColumnId.All(new TableId("Events")), value: null));
    Assert.Equal("column", exception.ParamName);
  }
}
