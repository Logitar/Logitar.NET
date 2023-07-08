using System.Reflection;

namespace Logitar.Data.UnitTests;

[Trait(Traits.Category, Categories.Unit)]
public class QueryBuilderTests
{
  [Fact(DisplayName = "Build: it constructs the correct query.")]
  public void Build_it_constructs_the_correct_query()
  {
    TableId table = new(schema: null, "MaTable", "x");
    QueryBuilderMock builder = new(table);

    IQuery query = builder.Build();
    string text = string.Join(Environment.NewLine,
      "DEPUIS «défaut»·«MaTable» «x»",
      string.Empty);
    Assert.Equal(text, query.Text);
  }

  [Fact(DisplayName = "Ctor: it constructs the correct query builder.")]
  public void Ctor_it_constructs_the_correct_query_builder()
  {
    TableId table = new(schema: null, "MyTable");
    QueryBuilder builder = new(table);

    FieldInfo? source = builder.GetType().GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(source);
    Assert.Equal(source.GetValue(builder), table);
  }

  [Fact(DisplayName = "From: it constructs the correct query builder.")]
  public void From_it_constructs_the_correct_query_builder()
  {
    TableId table = new(schema: null, "MyTable");
    QueryBuilder builder = QueryBuilder.From(table);

    FieldInfo? source = builder.GetType().GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(source);
    Assert.Equal(source.GetValue(builder), table);
  }
}
