﻿namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class UpdateBuilderTests
{
  private static readonly TableId _table = new("Events");

  private readonly UpdateBuilder _builder;

  public UpdateBuilderTests()
  {
    _builder = new()
    {
      Dialect = new DialectMock()
    };
  }

  [Fact(DisplayName = "Build: it throws InvalidOperationException when there are multiple tables updated.")]
  public void Build_it_throws_InvalidOperationException_when_there_are_multiple_tables_updated()
  {
    _builder.Set(
      new Update(new ColumnId("MaColonne1", _table), value: null),
      new Update(new ColumnId("MaColonne2", new TableId("MonAutreTable")), 123)
    );
    var exception = Assert.Throws<InvalidOperationException>(_builder.Build);
    Assert.StartsWith("An update command cannot update multiple tables.", exception.Message);
  }

  [Fact(DisplayName = "Build: it throws InvalidOperationException when there are no column update.")]
  public void Build_it_throws_InvalidOperationException_when_there_are_no_column_update()
  {
    var exception = Assert.Throws<InvalidOperationException>(_builder.Build);
    Assert.StartsWith("At least one column must be updated.", exception.Message);
  }

  [Fact(DisplayName = "Build: it throws InvalidOperationException when there is no table specified.")]
  public void Build_it_throws_InvalidOperationException_when_there_is_no_table_specified()
  {
    _builder.Set(
      new Update(new ColumnId("MaColonne1"), value: null),
      new Update(new ColumnId("MaColonne2"), 123)
    );
    var exception = Assert.Throws<InvalidOperationException>(_builder.Build);
    Assert.StartsWith("At least one column must specify a table.", exception.Message);
  }

  [Fact(DisplayName = "Build: it throws NotSupportedException when condition type is not supported.")]
  public void Build_it_throws_NotSupportedException_when_condition_type_is_not_supported()
  {
    _builder.Set(new Update(new ColumnId("MaColonne", _table), value: null));

    ConditionMock condition = new();
    _builder.Where(condition);
    var exception = Assert.Throws<NotSupportedException>(_builder.Build);
    string message = $"The condition '{condition}' is not supported.";
    Assert.Equal(message, exception.Message);
  }

  [Fact(DisplayName = "Build: it throws NotSupportedException when conditional operator is not supported.")]
  public void Build_it_throws_NotSupportedException_when_conditional_operator_is_not_supported()
  {
    _builder.Set(new Update(new ColumnId("MaColonne", _table), value: null));

    ConditionalOperatorMock @operator = new();
    OperatorCondition condition = new(new ColumnId("Test"), @operator);
    _builder.Where(condition);
    var exception = Assert.Throws<NotSupportedException>(_builder.Build);
    string message = $"The conditional operator '{@operator}' is not supported.";
    Assert.Equal(message, exception.Message);
  }

  [Fact(DisplayName = "It should build the correct update command.")]
  public void It_should_build_the_correct_update_command()
  {
    ICommand command = _builder
      .Set(
        new Update(new ColumnId("Status", _table), "Completed"),
        new Update(new ColumnId("Trace", _table), value: null)
      )
      .Where(new OperatorCondition(new ColumnId("Status", _table), Operators.IsEqualTo("InProgress")))
      .Build();

    string text = string.Join(Environment.NewLine, "MODIFIER «défaut»·«Events»",
      "DÉFINIR «défaut»·«Events»·«Status» == Πp0Θ, «défaut»·«Events»·«Trace» == NUL",
      "OÙ «défaut»·«Events»·«Status» == Πp1Θ");
    Assert.Equal(text, command.Text);

    Dictionary<string, IParameter> parameters = command.Parameters.Select(p => (IParameter)p)
      .ToDictionary(p => p.Name, p => p);
    Assert.Equal(2, parameters.Count);
    Assert.Equal("Completed", parameters["p0"].Value);
    Assert.Equal("InProgress", parameters["p1"].Value);
  }

  [Fact(DisplayName = "Set: it adds column updates to column update list.")]
  public void Set_it_adds_column_updates_to_column_update_list()
  {
    Update initialUpdate = new(new ColumnId("Status"), value: null);
    _builder.Set(initialUpdate);

    PropertyInfo? updates = _builder.GetType().GetProperty("Updates", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(updates);

    List<Update>? updateList = (List<Update>?)updates.GetValue(_builder);
    Assert.NotNull(updateList);
    Assert.Contains(initialUpdate, updateList);

    DateTime now = DateTime.Now;
    Update otherUpdate = new(new ColumnId("OccurredOn"), now);
    _builder.Set(otherUpdate);
    Assert.Contains(initialUpdate, updateList);
    Assert.Contains(otherUpdate, updateList);
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
