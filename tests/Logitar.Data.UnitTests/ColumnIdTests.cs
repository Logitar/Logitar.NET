namespace Logitar.Data.UnitTests;

[Trait(Traits.Category, Categories.Unit)]
public class ColumnIdTests
{
  [Theory(DisplayName = "All: it builds the correct column identifier.")]
  [InlineData(null)]
  [InlineData("MyTable")]
  public void All_it_builds_the_correct_column_identifier(string? tableName)
  {
    TableId? table = tableName == null ? null : new(tableName);
    ColumnId column = ColumnId.All(table);
    Assert.Null(column.Name);
    Assert.Same(table, column.Table);
  }

  [Theory(DisplayName = "Ctor: it builds the correct column identifier.")]
  [InlineData("MyColumn", null)]
  [InlineData("  MyColumn  ", "MyTable")]
  public void Ctor_it_builds_the_correct_column_identifier(string columnName, string? tableName)
  {
    TableId? table = tableName == null ? null : new(tableName);
    ColumnId column = new(columnName, table);
    Assert.Equal(columnName.Trim(), column.Name);
    Assert.Same(table, column.Table);
  }

  [Theory(DisplayName = "Ctor: it throws ArgumentException when column name is null, empty, or white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void Ctor_it_throws_ArgumentException_when_column_name_is_null_empty_or_white_space(string name)
  {
    var exception = Assert.Throws<ArgumentException>(() => new ColumnId(name));
    Assert.Equal("name", exception.ParamName);
  }
}
