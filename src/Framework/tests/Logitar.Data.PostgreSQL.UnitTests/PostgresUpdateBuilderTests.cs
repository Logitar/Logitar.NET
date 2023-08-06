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
    ICommand command = new PostgresUpdateBuilder()
      .Set(
        new Update(new ColumnId("Status", source), "Completed"),
        new Update(new ColumnId("Trace", source), value: null)
      )
      .Where(new OperatorCondition(new ColumnId("Status", source), PostgresOperators.IsLikeInsensitive("inprogress")))
      .Build();

    string text = string.Join(Environment.NewLine, @"UPDATE ""public"".""Events""",
      @"SET ""Status"" = @p0, ""Trace"" = NULL",
      @"WHERE ""public"".""Events"".""Status"" ILIKE @p1");
    Assert.Equal(text, command.Text);

    Dictionary<string, NpgsqlParameter> parameters = command.Parameters.Select(p => (NpgsqlParameter)p)
      .ToDictionary(p => p.ParameterName, p => p);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("Completed", parameters["p0"].Value);
    Assert.Equal("inprogress", parameters["p1"].Value);
  }
}
