using Npgsql;

namespace Logitar.Data.PostgreSQL.UnitTests;

[Trait(Traits.Category, Categories.Unit)]
public class PostgresQueryBuilderTests
{
  private readonly TableId _table = new("MaTable", "x");

  private readonly PostgresQueryBuilder _builder;

  public PostgresQueryBuilderTests()
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
      .LeftJoin(new ColumnId("ProjectId", new TableId("MesCommentaires")), new ColumnId("ProjectId", _table))
      .RightJoin(new ColumnId("UserId", new TableId("MesUtilisateurs")), new ColumnId("UserId", _table))
      .Where(new OrCondition(
        new OperatorCondition(priority, Operators.IsBetween(2, 4)),
        new OperatorCondition(priority, Operators.IsNull())
      ))
      .Where(new OperatorCondition(new ColumnId("Status"), Operators.IsNotEqualTo("Success")))
      .Where(new OperatorCondition(id, Operators.IsNotIn(7, 49, 343)))
      .Where(new OperatorCondition(new ColumnId("Trace"), Operators.IsLike("%fail%")))
      .Where(new OperatorCondition(new ColumnId("Trace"), PostgresOperators.IsNotLikeInsensitive("%fail%")))
      .OrderBy(
        new OrderBy(new ColumnId("DisplayName", _table)),
        new OrderBy(new ColumnId("UpdatedOn"), isDescending: true)
      )
      .Build();
    string text = string.Join(Environment.NewLine,
      @"SELECT *, ""x"".""MaTableId""",
      @"FROM ""public"".""MaTable"" ""x""",
      @"INNER JOIN ""public"".""MesTâches"" ""t"" ON ""t"".""MaTableId"" = ""x"".""MaTableId"" AND ""t"".""IsClosed"" = @p0",
      @"FULL JOIN ""public"".""MesProjets"" ON ""public"".""MesProjets"".""ProjectId"" = ""x"".""ProjectId""",
      @"LEFT JOIN ""public"".""MesCommentaires"" ON ""public"".""MesCommentaires"".""ProjectId"" = ""x"".""ProjectId""",
      @"RIGHT JOIN ""public"".""MesUtilisateurs"" ON ""public"".""MesUtilisateurs"".""UserId"" = ""x"".""UserId""",
      @"WHERE (""x"".""Priority"" BETWEEN @p1 AND @p2 OR ""x"".""Priority"" IS NULL) AND ""Status"" <> @p3 AND ""x"".""MaTableId"" NOT IN (@p4, @p5, @p6) AND ""Trace"" LIKE @p7 AND ""Trace"" NOT ILIKE @p8",
      @"ORDER BY ""x"".""DisplayName"" ASC, ""UpdatedOn"" DESC");
    Assert.Equal(text, query.Text);

    Dictionary<string, NpgsqlParameter> parameters = query.Parameters.Select(p => (NpgsqlParameter)p)
      .ToDictionary(p => p.ParameterName, p => p);
    Assert.Equal(9, parameters.Count);
    Assert.Equal(false, parameters["p0"].Value);
    Assert.Equal(2, parameters["p1"].Value);
    Assert.Equal(4, parameters["p2"].Value);
    Assert.Equal("Success", parameters["p3"].Value);
    Assert.Equal(7, parameters["p4"].Value);
    Assert.Equal(49, parameters["p5"].Value);
    Assert.Equal(343, parameters["p6"].Value);
    Assert.Equal("%fail%", parameters["p7"].Value);
    Assert.Equal("%fail%", parameters["p8"].Value);
  }

  [Fact(DisplayName = "Ctor: it should create the correct PostgresQueryBuilder.")]
  public void Ctor_it_should_create_the_correct_SqlServerQueryBuilder()
  {
    TableId source = new("MySchema", "MyTable", "x");
    PostgresQueryBuilder builder = new(source);

    PropertyInfo? sourceProperty = typeof(QueryBuilder).GetProperty("Source", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(sourceProperty);
    Assert.Same(source, sourceProperty.GetValue(builder));
  }

  [Fact(DisplayName = "From: it should create the correct PostgresQueryBuilder.")]
  public void From_it_should_create_the_correct_SqlServerQueryBuilder()
  {
    TableId source = new("MyTable");
    PostgresQueryBuilder builder = PostgresQueryBuilder.From(source);

    PropertyInfo? sourceProperty = typeof(QueryBuilder).GetProperty("Source", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(sourceProperty);
    Assert.Same(source, sourceProperty.GetValue(builder));
  }
}
