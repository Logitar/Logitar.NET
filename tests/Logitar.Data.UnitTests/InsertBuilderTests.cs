namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class InsertBuilderTests
{
  [Fact(DisplayName = "It should build the correct insert command.")]
  public void It_should_build_the_correct_insert_command()
  {
    object?[] row1 = new object?[] { 123, "SYSTEM", DateTime.Now };
    object?[] row2 = new object?[] { 456, null, DateTime.Now.AddHours(-1) };

    TableId events = new("Events");
    ICommand command = new InsertBuilderMock(new ColumnId("Id", events), new ColumnId("ActorId", events), new ColumnId("OccurredOn", events))
      .Value(row1)
      .Value(row2)
      .Build();

    string text = string.Join(Environment.NewLine, "INSÉRER DANS «défaut»·«Events» («Id», «ActorId», «OccurredOn») VALEURS",
      "(Πp0Θ, Πp1Θ, Πp2Θ),",
      "(Πp3Θ, NUL, Πp4Θ)");
    Assert.Equal(text, command.Text);

    Dictionary<string, IParameter> parameters = command.Parameters.Select(p => (IParameter)p)
      .ToDictionary(p => p.Name, p => p);
    Assert.Equal(5, parameters.Count);
    Assert.Equal(row1[0], parameters["p0"].Value);
    Assert.Equal(row1[1], parameters["p1"].Value);
    Assert.Equal(row1[2], parameters["p2"].Value);
    Assert.Equal(row2[0], parameters["p3"].Value);
    Assert.Equal(row2[2], parameters["p4"].Value);
  }

  [Theory(DisplayName = "It should construct the correct InsertBuilder.")]
  [InlineData("MaTable", "MaColonne1", "MaColonne2")]
  public void It_should_construct_the_correct_InsertBuilder(string tableName, string firstColumn, params string[] otherColumns)
  {
    TableId table = new(tableName);
    ColumnId[] columns = new[] { firstColumn }.Concat(otherColumns).Select(name => new ColumnId(name, table)).ToArray();
    InsertBuilderMock builder = new(columns);

    PropertyInfo? tableProperty = typeof(InsertBuilder).GetProperty("Table", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(tableProperty);
    Assert.Same(table, tableProperty.GetValue(builder));

    PropertyInfo? columnsProperty = typeof(InsertBuilder).GetProperty("Columns", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(columnsProperty);
    Assert.Same(columns, columnsProperty.GetValue(builder));
  }

  [Fact(DisplayName = "It should throw ArgumentException when a column name is null.")]
  public void It_should_throw_ArgumentException_when_a_column_name_is_null()
  {
    TableId table = new("MaTable");
    ColumnId firstColumn = new("MaColonne", table);
    ColumnId otherColumns = ColumnId.All(table);
    var exception = Assert.Throws<ArgumentException>(() => new InsertBuilderMock(firstColumn, otherColumns));
    Assert.Equal("columns", exception.ParamName);
    Assert.StartsWith("The column name is required (index: 1).", exception.Message);
  }

  [Fact(DisplayName = "It should throw ArgumentException when a column table is null.")]
  public void It_should_throw_ArgumentException_when_a_column_table_is_null()
  {
    ColumnId firstColumn = new("MaColonne");
    ColumnId otherColumn = new("MaTableId", new TableId("MaTable"));
    var exception = Assert.Throws<ArgumentException>(() => new InsertBuilderMock(firstColumn, otherColumn));
    Assert.Equal("columns", exception.ParamName);
    Assert.StartsWith("The column table cannot be null (index: 0).", exception.Message);
  }

  [Fact(DisplayName = "It should throw ArgumentException when inserting no value.")]
  public void It_should_throw_ArgumentException_when_inserting_no_value()
  {
    InsertBuilderMock builder = new(new ColumnId("MaColonne", new TableId("MaTable")));
    var exception = Assert.Throws<ArgumentException>(() => builder.Value());
    Assert.Equal("values", exception.ParamName);
    Assert.StartsWith("At least one value must be inserted.", exception.Message);
  }

  [Fact(DisplayName = "It should throw ArgumentException when no column has been specified.")]
  public void It_should_throw_ArgumentException_when_no_column_has_been_specified()
  {
    var exception = Assert.Throws<ArgumentException>(() => new InsertBuilderMock());
    Assert.Equal("columns", exception.ParamName);
    Assert.StartsWith("At least one column must be specified.", exception.Message);
  }

  [Fact(DisplayName = "It should throw ArgumentException when there are multiple tables.")]
  public void It_should_throw_ArgumentException_when_there_are_multiple_tables()
  {
    ColumnId firstColumn = new("MaColonne", new TableId("MaTable"));
    ColumnId otherColumn = new("MonAutreColonne", new TableId("MonAutreTable"));
    var exception = Assert.Throws<ArgumentException>(() => new InsertBuilderMock(firstColumn, otherColumn));
    Assert.Equal("columns", exception.ParamName);
    Assert.StartsWith("An insert command cannot insert into multiple tables.", exception.Message);
  }

  [Fact(DisplayName = "It should throw InvalidOperationException when building without any row.")]
  public void It_should_throw_InvalidOperationException_when_building_without_any_row()
  {
    InsertBuilderMock builder = new(new ColumnId("MaColonne", new TableId("MaTable")));
    var exception = Assert.Throws<InvalidOperationException>(builder.Build);
    Assert.Equal("At least one row must be inserted.", exception.Message);
  }
}
