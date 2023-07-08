namespace Logitar.Data.UnitTests;

[Trait(Traits.Category, Categories.Unit)]
public class QueryBuilderTests
{
  private readonly TableId _table = new("MaTable", "x");

  private readonly QueryBuilder _builder;

  public QueryBuilderTests()
  {
    _builder = new(_table);
  }

  [Fact(DisplayName = "Build: it constructs the correct query.")]
  public void Build_it_constructs_the_correct_query()
  {
    QueryBuilderMock builder = new(_table);

    IQuery query = builder
      .Select(ColumnId.All(), new ColumnId($"{_table.Table}Id", _table))
      .OrderBy(
        new OrderBy(new ColumnId("DisplayName", _table)),
        new OrderBy(new ColumnId("UpdatedOn"), isDescending: true)
      )
      .Build();
    string text = string.Join(Environment.NewLine,
      "SÉLECTIONNER Ω, «x»·«MaTableId»",
      "DEPUIS «défaut»·«MaTable» «x»",
      "ORDONNER PAR «x»·«DisplayName» ↑ PUIS PAR «UpdatedOn» ↓",
      string.Empty);
    Assert.Equal(text, query.Text);
  }

  [Fact(DisplayName = "Ctor: it constructs the correct query builder.")]
  public void Ctor_it_constructs_the_correct_query_builder()
  {
    FieldInfo? _source = _builder.GetType().GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(_source);
    Assert.Equal(_source.GetValue(_builder), _table);
  }

  [Fact(DisplayName = "Ctor: it throws ArgumentException when table name is null.")]
  public void Ctor_it_throws_ArgumentException_when_table_name_is_null()
  {
    var exception = Assert.Throws<ArgumentException>(() => new QueryBuilder(TableId.FromAlias("alias")));
    Assert.Equal("source", exception.ParamName);
  }

  [Fact(DisplayName = "From: it constructs the correct query builder.")]
  public void From_it_constructs_the_correct_query_builder()
  {
    FieldInfo? _source = _builder.GetType().GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(_source);
    Assert.Equal(_source.GetValue(_builder), _table);
  }

  [Fact(DisplayName = "OrderBy: it replaces the order by list.")]
  public void OrderBy_it_replaces_the_order_by_list()
  {
    FieldInfo? _orderBy = _builder.GetType().GetField("_orderBy", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(_orderBy);
    List<OrderBy> orderByList;

    OrderBy oldOrderBy = new(new ColumnId("MyColumn", _table));
    _builder.OrderBy(oldOrderBy);
    orderByList = (List<OrderBy>)_orderBy.GetValue(_builder)!;
    Assert.NotNull(orderByList);
    Assert.Same(oldOrderBy, orderByList.Single());

    OrderBy[] newOrderBy = new[]
    {
      new OrderBy(new ColumnId("DisplayName")),
      new OrderBy(new ColumnId("UpdatedOn"), isDescending: true)
    };
    _builder.OrderBy(newOrderBy);
    orderByList = (List<OrderBy>)_orderBy.GetValue(_builder)!;
    Assert.NotNull(orderByList);
    Assert.True(newOrderBy.SequenceEqual(orderByList));
  }

  [Fact(DisplayName = "Select: it adds column to selection.")]
  public void Select_it_adds_column_to_selection()
  {
    ColumnId column = ColumnId.All(_table);
    _builder.Select(column);

    FieldInfo? _selections = _builder.GetType().GetField("_selections", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(_selections);

    List<ColumnId>? selections = (List<ColumnId>?)_selections.GetValue(_builder);
    Assert.NotNull(selections);
    Assert.Contains(column, selections);
  }
}
