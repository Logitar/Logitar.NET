﻿using Microsoft.Data.SqlClient;

namespace Logitar.Data.SqlServer;

[Trait(Traits.Category, Categories.Unit)]
public class SqlServerQueryBuilderTests
{
  private readonly TableId _table = new("MaTable", "x");

  private readonly SqlServerQueryBuilder _builder;

  public SqlServerQueryBuilderTests()
  {
    _builder = new(_table);
  }

  [Fact(DisplayName = "Build: it constructs the correct query.")]
  public void Build_it_constructs_the_correct_query()
  {
    ColumnId priority = new("Priority", _table);
    ColumnId id = new($"{_table.Table}Id", _table);

    IQuery query = _builder
      .Select(ColumnId.All(), id)
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
      "SELECT *, [x].[MaTableId]",
      "FROM [dbo].[MaTable] [x]",
      "WHERE ([x].[Priority] BETWEEN @p0 AND @p1 OR [x].[Priority] IS NULL) AND [Status] <> @p2 AND [x].[MaTableId] NOT IN (@p3, @p4, @p5) AND [Trace] LIKE @p6",
      "ORDER BY [x].[DisplayName] ASC THEN BY [UpdatedOn] DESC");
    Assert.Equal(text, query.Text);

    Dictionary<string, SqlParameter> parameters = query.Parameters.Select(p => (SqlParameter)p)
      .ToDictionary(p => p.ParameterName, p => p);
    Assert.Equal(7, parameters.Count);
    Assert.Equal(2, parameters["p0"].Value);
    Assert.Equal(4, parameters["p1"].Value);
    Assert.Equal("Success", parameters["p2"].Value);
    Assert.Equal(7, parameters["p3"].Value);
    Assert.Equal(49, parameters["p4"].Value);
    Assert.Equal(343, parameters["p5"].Value);
    Assert.Equal("%fail%", parameters["p6"].Value);
  }

  [Fact(DisplayName = "Ctor: it should create the correct SqlServerQueryBuilder.")]
  public void Ctor_it_should_create_the_correct_SqlServerQueryBuilder()
  {
    TableId source = new("MySchema", "MyTable", "x");
    SqlServerQueryBuilder builder = new(source);

    PropertyInfo? sourceProperty = typeof(QueryBuilder).GetProperty("Source", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(sourceProperty);
    Assert.Same(source, sourceProperty.GetValue(builder));
  }

  [Fact(DisplayName = "From: it should create the correct SqlServerQueryBuilder.")]
  public void From_it_should_create_the_correct_SqlServerQueryBuilder()
  {
    TableId source = new("MyTable");
    SqlServerQueryBuilder builder = SqlServerQueryBuilder.From(source);

    PropertyInfo? sourceProperty = typeof(QueryBuilder).GetProperty("Source", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(sourceProperty);
    Assert.Same(source, sourceProperty.GetValue(builder));
  }
}
