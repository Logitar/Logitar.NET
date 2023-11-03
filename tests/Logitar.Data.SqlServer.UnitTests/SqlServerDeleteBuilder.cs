using Microsoft.Data.SqlClient;

namespace Logitar.Data.SqlServer;

[Trait(Traits.Category, Categories.Unit)]
public class SqlServerDeleteBuilderTests
{
  [Fact(DisplayName = "Build: it constructs the correct command.")]
  public void Build_it_constructs_the_correct_command()
  {
    DateTime moment = DateTime.Now.AddYears(-1);
    TableId source = new("Events");
    ICommand command = SqlServerDeleteBuilder.From(source)
      .Where(
        new OperatorCondition(new ColumnId("Status", source), Operators.IsEqualTo("Completed")),
        new OperatorCondition(new ColumnId("OccurredOn", source), Operators.IsLessThanOrEqualTo(moment))
      )
      .Build();

    string text = string.Join(Environment.NewLine, "DELETE FROM [dbo].[Events]",
      "WHERE [dbo].[Events].[Status] = @p0 AND [dbo].[Events].[OccurredOn] <= @p1");
    Assert.Equal(text, command.Text);

    Dictionary<string, SqlParameter> parameters = command.Parameters.Select(p => (SqlParameter)p)
      .ToDictionary(p => p.ParameterName, p => p);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("Completed", parameters["p0"].Value);
    Assert.Equal(moment, parameters["p1"].Value);
  }

  [Fact(DisplayName = "Ctor: it should create the correct SqlServerDeleteBuilder.")]
  public void Ctor_it_should_create_the_correct_SqlServerDeleteBuilder()
  {
    TableId source = new("MySchema", "MyTable", "x");
    SqlServerDeleteBuilder builder = new(source);

    PropertyInfo? sourceProperty = typeof(DeleteBuilder).GetProperty("Source", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(sourceProperty);
    Assert.Same(source, sourceProperty.GetValue(builder));
  }

  [Fact(DisplayName = "From: it should create the correct SqlServerDeleteBuilder.")]
  public void From_it_should_create_the_correct_SqlServerDeleteBuilder()
  {
    TableId source = new("MyTable");
    SqlServerDeleteBuilder builder = SqlServerDeleteBuilder.From(source);

    PropertyInfo? sourceProperty = typeof(DeleteBuilder).GetProperty("Source", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(sourceProperty);
    Assert.Same(source, sourceProperty.GetValue(builder));
  }
}
