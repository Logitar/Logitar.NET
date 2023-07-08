using System.Reflection;

namespace Logitar.Data.UnitTests;

[Trait(Traits.Category, Categories.Unit)]
public class QueryBuilderTests
{
  private readonly TableId _table = new(schema: null, "MaTable", "x");

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
      .Build();
    string text = string.Join(Environment.NewLine,
      "SÉLECTIONNER Ω, «x»·«MaTableId»",
      "DEPUIS «défaut»·«MaTable» «x»",
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

  [Fact(DisplayName = "From: it constructs the correct query builder.")]
  public void From_it_constructs_the_correct_query_builder()
  {
    FieldInfo? _source = _builder.GetType().GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(_source);
    Assert.Equal(_source.GetValue(_builder), _table);
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
