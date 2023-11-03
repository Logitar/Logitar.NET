using Npgsql;

namespace Logitar.Data.PostgreSQL;

[Trait(Traits.Category, Categories.Unit)]
public class PostgresInsertBuilderTests
{
  [Fact(DisplayName = "It should build the correct insert command.")]
  public void It_should_build_the_correct_insert_command()
  {
    object?[] row1 = new object?[] { 123, "SYSTEM", DateTime.Now };
    object?[] row2 = new object?[] { 456, null, DateTime.Now.AddHours(-1) };

    TableId events = new("Events");
    ICommand command = PostgresInsertBuilder.Into(new ColumnId("Id", events), new ColumnId("ActorId", events), new ColumnId("OccurredOn", events))
      .Value(row1)
      .Value(row2)
      .Build();

    string text = string.Join(Environment.NewLine, @"INSERT INTO ""public"".""Events"" (""Id"", ""ActorId"", ""OccurredOn"") VALUES",
      "(@p0, @p1, @p2),",
      "(@p3, NULL, @p4)");
    Assert.Equal(text, command.Text);

    Dictionary<string, NpgsqlParameter> parameters = command.Parameters.Select(p => (NpgsqlParameter)p)
      .ToDictionary(p => p.ParameterName, p => p);
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
    PostgresInsertBuilder builder = new(columns);

    PropertyInfo? tableProperty = typeof(InsertBuilder).GetProperty("Table", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(tableProperty);
    Assert.Same(table, tableProperty.GetValue(builder));

    PropertyInfo? columnsProperty = typeof(InsertBuilder).GetProperty("Columns", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(columnsProperty);
    Assert.Same(columns, columnsProperty.GetValue(builder));
  }

  [Theory(DisplayName = "Into: it should construct the correct InsertBuilder.")]
  [InlineData("MaTable", "MaColonne1", "MaColonne2")]
  public void Into_it_should_construct_the_correct_InsertBuilder(string tableName, string firstColumn, params string[] otherColumns)
  {
    TableId table = new(tableName);
    ColumnId[] columns = new[] { firstColumn }.Concat(otherColumns).Select(name => new ColumnId(name, table)).ToArray();
    PostgresInsertBuilder builder = PostgresInsertBuilder.Into(columns);

    PropertyInfo? tableProperty = typeof(InsertBuilder).GetProperty("Table", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(tableProperty);
    Assert.Same(table, tableProperty.GetValue(builder));

    PropertyInfo? columnsProperty = typeof(InsertBuilder).GetProperty("Columns", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(columnsProperty);
    Assert.Same(columns, columnsProperty.GetValue(builder));
  }
}
