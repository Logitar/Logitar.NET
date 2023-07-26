namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class QueryBuilderTests
{
  private readonly TableId _table = new("MaTable", "x");

  private readonly QueryBuilderMock _builder;

  public QueryBuilderTests()
  {
    _builder = new(_table);
  }

  [Fact(DisplayName = "Build: it constructs the correct query.")]
  public void Build_it_constructs_the_correct_query()
  {
    ColumnId priority = new("Priority", _table);
    ColumnId id = new($"{_table.Table}Id", _table);
    TableId tasks = new("MesTâches", "t");
    Assert.NotNull(id.Name);

    IQuery query = _builder
      .Select(ColumnId.All(), id)
      .Join(new ColumnId(id.Name, tasks), id, new OperatorCondition(new ColumnId("IsClosed", tasks), Operators.IsEqualTo(false)))
      .FullJoin(new ColumnId("ProjectId", new TableId("MesProjets")), new ColumnId("ProjectId", _table))
      .Where(new OrCondition(
        new OperatorCondition(priority, Operators.IsBetween(2, 4)),
        new OperatorCondition(priority, Operators.IsNull())
      ))
      .Where(new OperatorCondition(new ColumnId("Status"), Operators.IsNotEqualTo("Success")))
      .Where(new OperatorCondition(id, Operators.IsNotIn(7, 49, 343)))
      .Where(new OperatorCondition(new ColumnId("Trace"), Operators.IsLike("%fail%")))
      .OrderBy(
        new OrderBy(new ColumnId("DisplayName", _table)),
        new OrderBy(new ColumnId("UpdatedOn"), isDescending: true)
      )
      .Build();
    string text = string.Join(Environment.NewLine,
      "SÉLECTIONNER Ω, «x»·«MaTableId»",
      "DEPUIS «défaut»·«MaTable» «x»",
      "JOINDRE À L'INTÉRIEUR «défaut»·«MesTâches» «t» SUR «t»·«MaTableId» == «x»·«MaTableId» ET «t»·«IsClosed» == Πp0Θ",
      "JOINDRE COMPLÈTEMENT «défaut»·«MesProjets» SUR «défaut»·«MesProjets»·«ProjectId» == «x»·«ProjectId»",
      "OÙ («x»·«Priority» DANS L'INTERVALLE Πp1Θ ET Πp2Θ OU «x»·«Priority» EST NUL) ET «Status» != Πp3Θ ET «x»·«MaTableId» NON DANS (Πp4Θ, Πp5Θ, Πp6Θ) ET «Trace» COMME Πp7Θ",
      "ORDONNER PAR «x»·«DisplayName» ↑ PUIS PAR «UpdatedOn» ↓");
    Assert.Equal(text, query.Text);

    Dictionary<string, IParameter> parameters = query.Parameters.Select(p => (IParameter)p)
      .ToDictionary(p => p.Name, p => p);
    Assert.Equal(8, parameters.Count);
    Assert.Equal(false, parameters["p0"].Value);
    Assert.Equal(2, parameters["p1"].Value);
    Assert.Equal(4, parameters["p2"].Value);
    Assert.Equal("Success", parameters["p3"].Value);
    Assert.Equal(7, parameters["p4"].Value);
    Assert.Equal(49, parameters["p5"].Value);
    Assert.Equal(343, parameters["p6"].Value);
    Assert.Equal("%fail%", parameters["p7"].Value);
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

  [Fact(DisplayName = "Ctor: it constructs the correct query builder.")]
  public void Ctor_it_constructs_the_correct_query_builder()
  {
    PropertyInfo? source = _builder.GetType().GetProperty("Source", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(source);
    Assert.Equal(source.GetValue(_builder), _table);
  }

  [Fact(DisplayName = "Ctor: it throws ArgumentException when table name is null.")]
  public void Ctor_it_throws_ArgumentException_when_table_name_is_null()
  {
    var exception = Assert.Throws<ArgumentException>(() => new QueryBuilderMock(TableId.FromAlias("alias")));
    Assert.Equal("source", exception.ParamName);
  }

  [Fact(DisplayName = "From: it constructs the correct query builder.")]
  public void From_it_constructs_the_correct_query_builder()
  {
    PropertyInfo? source = _builder.GetType().GetProperty("Source", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(source);
    Assert.Equal(source.GetValue(_builder), _table);
  }

  [Fact(DisplayName = "Join: it adds joins to join list.")]
  public void Join_it_adds_joins_to_join_list()
  {
    TableId userRoles = new("UserRoles");
    Join initialJoin = new(left: new ColumnId("UserId", userRoles),
      right: new ColumnId("UserId", new TableId("Users")),
      condition: new OperatorCondition(new ColumnId("IsActive", userRoles), Operators.IsEqualTo(true)));
    _builder.Join(initialJoin);

    PropertyInfo? joins = _builder.GetType().GetProperty("Joins", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(joins);

    List<Join>? joinList = (List<Join>?)joins.GetValue(_builder);
    Assert.NotNull(joinList);
    Assert.Contains(initialJoin, joinList);

    Join otherJoin = new(left: new ColumnId("RoleId", new TableId("Roles")),
      right: new ColumnId("RoleId", userRoles));
    _builder.Join(otherJoin);
    Assert.Contains(initialJoin, joinList);
    Assert.Contains(otherJoin, joinList);
  }

  [Fact(DisplayName = "OrderBy: it replaces the order by list.")]
  public void OrderBy_it_replaces_the_order_by_list()
  {
    PropertyInfo? orderBy = _builder.GetType().GetProperty("OrderByList", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(orderBy);
    List<OrderBy> orderByList;

    OrderBy oldOrderBy = new(new ColumnId("MyColumn", _table));
    _builder.OrderBy(oldOrderBy);
    orderByList = (List<OrderBy>)orderBy.GetValue(_builder)!;
    Assert.NotNull(orderByList);
    Assert.Same(oldOrderBy, orderByList.Single());

    OrderBy[] newOrderBy = new[]
    {
      new OrderBy(new ColumnId("DisplayName")),
      new OrderBy(new ColumnId("UpdatedOn"), isDescending: true)
    };
    _builder.OrderBy(newOrderBy);
    orderByList = (List<OrderBy>)orderBy.GetValue(_builder)!;
    Assert.NotNull(orderByList);
    Assert.Equal(newOrderBy, orderByList);
  }

  [Fact(DisplayName = "Select: it adds column to selection.")]
  public void Select_it_adds_column_to_selection()
  {
    ColumnId initialColumn = ColumnId.All(_table);
    _builder.Select(initialColumn);

    PropertyInfo? selections = _builder.GetType().GetProperty("Selections", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(selections);

    List<ColumnId>? selectionList = (List<ColumnId>?)selections.GetValue(_builder);
    Assert.NotNull(selectionList);
    Assert.Contains(initialColumn, selectionList);

    ColumnId otherColumn = new("Test");
    _builder.Select(otherColumn);
    Assert.Contains(initialColumn, selectionList);
    Assert.Contains(otherColumn, selectionList);
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
