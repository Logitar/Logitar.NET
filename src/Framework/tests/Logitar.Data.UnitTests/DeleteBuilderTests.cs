namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class DeleteBuilderTests
{
  private static readonly TableId _table = new("Events");

  private readonly DeleteBuilder _builder;

  public DeleteBuilderTests()
  {
    _builder = new(_table)
    {
      Dialect = new DialectMock()
    };
  }

  [Fact(DisplayName = "Build: it throws NotSupportedException when condition type is not supported.")]
  public void Build_it_throws_NotSupportedException_when_condition_type_is_not_supported()
  {
    ConditionMock condition = new();
    _builder.Where(condition);
    var exception = Assert.Throws<NotSupportedException>(_builder.Build);
    string message = $"The condition '{condition}' is not supported.";
    Assert.Equal(message, exception.Message);
  }

  [Fact(DisplayName = "Build: it throws NotSupportedException when conditional operator is not supported.")]
  public void Build_it_throws_NotSupportedException_when_conditional_operator_is_not_supported()
  {
    ConditionalOperatorMock @operator = new();
    OperatorCondition condition = new(new ColumnId("Test"), @operator);
    _builder.Where(condition);
    var exception = Assert.Throws<NotSupportedException>(_builder.Build);
    string message = $"The conditional operator '{@operator}' is not supported.";
    Assert.Equal(message, exception.Message);
  }

  [Fact(DisplayName = "Ctor: it throws ArgumentException when table name is null.")]
  public void Ctor_it_throws_ArgumentException_when_table_name_is_null()
  {
    var exception = Assert.Throws<ArgumentException>(() => new DeleteBuilder(TableId.FromAlias("alias")));
    Assert.Equal("source", exception.ParamName);
  }

  [Fact(DisplayName = "It should build the correct delete command.")]
  public void It_should_build_the_correct_delete_command()
  {
    DateTime moment = DateTime.Now.AddYears(-1);
    ICommand command = _builder
      .Where(
        new OperatorCondition(new ColumnId("Status", _table), Operators.IsEqualTo("Completed")),
        new OperatorCondition(new ColumnId("OccurredOn", _table), Operators.IsLessThanOrEqualTo(moment))
      )
      .Build();

    string text = string.Join(Environment.NewLine, "SUPPRIMER DE «défaut»·«Events»",
      "OÙ «défaut»·«Events»·«Status» == Πp0Θ ET «défaut»·«Events»·«OccurredOn» <<== Πp1Θ");
    Assert.Equal(text, command.Text);

    Dictionary<string, IParameter> parameters = command.Parameters.Select(p => (IParameter)p)
      .ToDictionary(p => p.Name, p => p);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("Completed", parameters["p0"].Value);
    Assert.Equal(moment, parameters["p1"].Value);
  }

  [Fact(DisplayName = "It should construct the correct DeleteBuilder.")]
  public void It_should_construct_the_correct_DeleteBuilder()
  {
    PropertyInfo? source = _builder.GetType().GetProperty("Source", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(source);
    Assert.Equal(source.GetValue(_builder), _table);
  }

  [Fact(DisplayName = "Where: it adds conditions to condition list.")]
  public void Where_it_adds_conditions_to_condition_list()
  {
    OperatorCondition initialCondition = new(new ColumnId("Status"), Operators.IsNotNull());
    _builder.Where(initialCondition);

    PropertyInfo? conditions = _builder.GetType().GetProperty("Conditions", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(conditions);

    List<Condition>? conditionList = (List<Condition>?)conditions.GetValue(_builder);
    Assert.NotNull(conditionList);
    Assert.Contains(initialCondition, conditionList);

    OperatorCondition otherCondition = new(new ColumnId("Test"), Operators.IsNotLike("%fail%"));
    _builder.Where(otherCondition);
    Assert.Contains(initialCondition, conditionList);
    Assert.Contains(otherCondition, conditionList);
  }
}
