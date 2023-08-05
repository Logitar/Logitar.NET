using Npgsql;

namespace Logitar.Data.PostgreSQL;

[Trait(Traits.Category, Categories.Unit)]
public class PostgresUpdateBuilderTests
{
  [Fact(DisplayName = "Build: it constructs the correct command.")]
  public void Build_it_constructs_the_correct_command()
  {
    DateTime moment = DateTime.Now.AddYears(-1);
    TableId source = new("Events");
    ICommand command = PostgresUpdateBuilder.From(source)
      .Set(
        new Update(new ColumnId("Status", source), "Completed"),
        new Update(new ColumnId("Trace", source), value: null)
      )
      .Where(new OperatorCondition(new ColumnId("Status", source), Operators.IsEqualTo("InProgress")))
      .Build();

    string text = string.Join(Environment.NewLine, @"UPDATE ""public"".""Events""",
      @"SET ""public"".""Events"".""Status"" = @p0, ""public"".""Events"".""Trace"" = NULL",
      @"WHERE ""public"".""Events"".""Status"" = @p1");
    Assert.Equal(text, command.Text);

    Dictionary<string, NpgsqlParameter> parameters = command.Parameters.Select(p => (NpgsqlParameter)p)
      .ToDictionary(p => p.ParameterName, p => p);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("Completed", parameters["p0"].Value);
    Assert.Equal("InProgress", parameters["p1"].Value);
  }

  [Fact(DisplayName = "Ctor: it should create the correct PostgresUpdateBuilder.")]
  public void Ctor_it_should_create_the_correct_PostgresUpdateBuilder()
  {
    TableId source = new("MySchema", "MyTable", "x");
    PostgresUpdateBuilder builder = new(source);

    PropertyInfo? sourceProperty = typeof(UpdateBuilder).GetProperty("Source", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(sourceProperty);
    Assert.Same(source, sourceProperty.GetValue(builder));
  }

  [Fact(DisplayName = "From: it should create the correct PostgresUpdateBuilder.")]
  public void From_it_should_create_the_correct_PostgresUpdateBuilder()
  {
    TableId source = new("MyTable");
    PostgresUpdateBuilder builder = PostgresUpdateBuilder.From(source);

    PropertyInfo? sourceProperty = typeof(UpdateBuilder).GetProperty("Source", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(sourceProperty);
    Assert.Same(source, sourceProperty.GetValue(builder));
  }
}
