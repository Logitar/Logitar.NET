using Npgsql;

namespace Logitar.Data.PostgreSQL;

[Trait(Traits.Category, Categories.Unit)]
public class PostgresDeleteBuilderTests
{
  [Fact(DisplayName = "Build: it constructs the correct command.")]
  public void Build_it_constructs_the_correct_command()
  {
    DateTime moment = DateTime.Now.AddYears(-1);
    TableId source = new("Events");
    ICommand command = PostgresDeleteBuilder.From(source)
      .Where(
        new OperatorCondition(new ColumnId("Status", source), Operators.IsEqualTo("Completed")),
        new OperatorCondition(new ColumnId("OccurredOn", source), Operators.IsLessThanOrEqualTo(moment))
      )
      .Build();

    string text = string.Join(Environment.NewLine, @"DELETE FROM ""public"".""Events""",
      @"WHERE ""public"".""Events"".""Status"" = @p0 AND ""public"".""Events"".""OccurredOn"" <= @p1");
    Assert.Equal(text, command.Text);

    Dictionary<string, NpgsqlParameter> parameters = command.Parameters.Select(p => (NpgsqlParameter)p)
      .ToDictionary(p => p.ParameterName, p => p);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("Completed", parameters["p0"].Value);
    Assert.Equal(moment, parameters["p1"].Value);
  }

  [Fact(DisplayName = "Ctor: it should create the correct PostgresDeleteBuilder.")]
  public void Ctor_it_should_create_the_correct_PostgresDeleteBuilder()
  {
    TableId source = new("MySchema", "MyTable", "x");
    PostgresDeleteBuilder builder = new(source);

    PropertyInfo? sourceProperty = typeof(DeleteBuilder).GetProperty("Source", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(sourceProperty);
    Assert.Same(source, sourceProperty.GetValue(builder));
  }

  [Fact(DisplayName = "From: it should create the correct PostgresDeleteBuilder.")]
  public void From_it_should_create_the_correct_PostgresDeleteBuilder()
  {
    TableId source = new("MyTable");
    PostgresDeleteBuilder builder = PostgresDeleteBuilder.From(source);

    PropertyInfo? sourceProperty = typeof(DeleteBuilder).GetProperty("Source", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(sourceProperty);
    Assert.Same(source, sourceProperty.GetValue(builder));
  }
}
