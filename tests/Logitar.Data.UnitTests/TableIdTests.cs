namespace Logitar.Data.UnitTests;

[Trait(Traits.Category, Categories.Unit)]
public class TableIdTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct table identifier.")]
  [InlineData(null, "  MyTable  ", null)]
  [InlineData("", "MyTable", null)]
  [InlineData("  ", "MyTable", null)]
  [InlineData("MySchema", "MyTable", null)]
  [InlineData("MySchema", "MyTable", "")]
  [InlineData("MySchema", "MyTable", "  ")]
  [InlineData(null, "MyTable", "x")]
  [InlineData("MySchema", "MyTable", "x")]
  public void Ctor_it_constructs_the_correct_table_identifier(string? schema, string table, string? alias)
  {
    TableId id = new(schema, table, alias);
    Assert.Equal(schema?.CleanTrim(), id.Schema);
    Assert.Equal(table.Trim(), id.Table);
    Assert.Equal(alias?.CleanTrim(), id.Alias);
  }

  [Theory(DisplayName = "Ctor: it throws ArgumentException when table is null empty or only white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void Ctor_it_throws_ArgumentException_when_table_is_null_empty_or_only_white_space(string table)
  {
    var exception = Assert.Throws<ArgumentException>(() => new TableId(schema: null, table));
    Assert.Equal("table", exception.ParamName);
  }
}
