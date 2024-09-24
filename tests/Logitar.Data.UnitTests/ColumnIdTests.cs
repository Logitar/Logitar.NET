namespace Logitar.Data;

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

  [Theory(DisplayName = "Ctor: it constructs the correct column identifier.")]
  [InlineData("MyColumn", null, null)]
  [InlineData("  MyColumn  ", "MyTable", null)]
  [InlineData("MyColumn", "MyTable", "  Value  ")]
  public void Ctor_it_constructs_the_correct_column_identifier(string columnName, string? tableName, string? alias)
  {
    TableId? table = tableName == null ? null : new(tableName);
    ColumnId column = new(columnName, table, alias);
    Assert.Equal(columnName.Trim(), column.Name);
    Assert.Same(table, column.Table);

    if (string.IsNullOrWhiteSpace(alias))
    {
      Assert.Null(column.Alias);
    }
    else
    {
      Assert.Equal(alias.Trim(), column.Alias);
    }
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
