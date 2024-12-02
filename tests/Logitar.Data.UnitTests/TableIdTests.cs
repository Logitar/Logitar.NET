namespace Logitar.Data;

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

  [Theory(DisplayName = "Ctor: it throws ArgumentException when table identifier is not valid.")]
  [InlineData("MySchema.MyTable.")]
  public void Ctor_it_throws_ArgumentException_when_table_identifier_is_not_valid(string identifier)
  {
    var exception = Assert.Throws<ArgumentException>(() => new TableId(identifier));
    Assert.Equal("identifier", exception.ParamName);
  }

  [Theory(DisplayName = "Ctor: it throws ArgumentException when table is null, empty, or only white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void Ctor_it_throws_ArgumentException_when_table_is_null_empty_or_only_white_space(string? table)
  {
    var exception = Assert.Throws<ArgumentException>(() => new TableId(schema: null, table!));
    Assert.Equal("table", exception.ParamName);
  }

  [Theory(DisplayName = "FromAlias: it constructs the correct table identifier.")]
  [InlineData("x")]
  [InlineData("  x  ")]
  public void FromAlias_it_constructs_the_correct_table_identifier(string alias)
  {
    TableId table = TableId.FromAlias(alias);
    Assert.Null(table.Schema);
    Assert.Null(table.Table);
    Assert.Equal(alias.Trim(), table.Alias);
  }

  [Theory(DisplayName = "FromAlias: it throws ArgumentException when alias is null, empty, or only white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void FromAlias_it_throws_ArgumentException_when_alias_is_null_empty_or_only_white_space(string? alias)
  {
    var exception = Assert.Throws<ArgumentException>(() => TableId.FromAlias(alias!));
    Assert.Equal("alias", exception.ParamName);
  }

  [Theory(DisplayName = "Identifier Ctor: it constructs the correct table identifier.")]
  [InlineData(null, "  MyTable  ", null)]
  [InlineData("", "MyTable", null)]
  [InlineData("  ", "MyTable", null)]
  [InlineData("MySchema", "MyTable", null)]
  [InlineData("MySchema", "MyTable", "")]
  [InlineData("MySchema", "MyTable", "  ")]
  [InlineData(null, "MyTable", "x")]
  [InlineData("MySchema", "MyTable", "x")]
  public void Identifier_Ctor_it_constructs_the_correct_table_identifier(string? schema, string table, string? alias)
  {
    string identifier = string.Join(TableId.Separator, schema, table);
    TableId id = new(identifier, alias);
    Assert.Equal(schema?.CleanTrim(), id.Schema);
    Assert.Equal(table.Trim(), id.Table);
    Assert.Equal(alias?.CleanTrim(), id.Alias);
  }

  [Theory(DisplayName = "Identifier Ctor: it throws ArgumentException when table is null, empty, or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  [InlineData("  .")]
  [InlineData("  .  ")]
  public void Identifier_Ctor_it_throws_ArgumentException_when_table_is_null_empty_or_only_white_space(string identifier)
  {
    var exception = Assert.Throws<ArgumentException>(() => new TableId(identifier));
    Assert.Equal("identifier", exception.ParamName);
  }
}
