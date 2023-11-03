namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class OrderByTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct order by.")]
  [InlineData("MyColumn", false)]
  [InlineData("MyColumn", true)]
  public void Ctor_it_constructs_the_correct_order_by(string columnName, bool isDescending)
  {
    ColumnId column = new(columnName);
    OrderBy orderBy = new(column, isDescending);
    Assert.Same(column, orderBy.Column);
    Assert.Equal(isDescending, orderBy.IsDescending);
  }

  [Fact(DisplayName = "Ctor: it throws ArgumentException when column name is null.")]
  public void Ctor_it_throws_ArgumentException_when_column_name_is_null()
  {
    var exception = Assert.Throws<ArgumentException>(() => new OrderBy(ColumnId.All()));
    Assert.Equal("column", exception.ParamName);
  }
}
