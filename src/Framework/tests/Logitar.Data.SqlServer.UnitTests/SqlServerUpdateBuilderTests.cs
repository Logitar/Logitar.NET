using Microsoft.Data.SqlClient;

namespace Logitar.Data.SqlServer;

[Trait(Traits.Category, Categories.Unit)]
public class SqlServerUpdateBuilderTests
{
  [Fact(DisplayName = "Build: it constructs the correct command.")]
  public void Build_it_constructs_the_correct_command()
  {
    DateTime moment = DateTime.Now.AddYears(-1);
    TableId source = new("Events");
    ICommand command = new SqlServerUpdateBuilder()
      .Set(
        new Update(new ColumnId("Status", source), "Completed"),
        new Update(new ColumnId("Trace", source), value: null)
      )
      .Where(new OperatorCondition(new ColumnId("Status", source), Operators.IsEqualTo("InProgress")))
      .Build();

    string text = string.Join(Environment.NewLine, "UPDATE [dbo].[Events]",
      "SET [dbo].[Events].[Status] = @p0, [dbo].[Events].[Trace] = NULL",
      "WHERE [dbo].[Events].[Status] = @p1");
    Assert.Equal(text, command.Text);

    Dictionary<string, SqlParameter> parameters = command.Parameters.Select(p => (SqlParameter)p)
      .ToDictionary(p => p.ParameterName, p => p);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("Completed", parameters["p0"].Value);
    Assert.Equal("InProgress", parameters["p1"].Value);
  }
}
