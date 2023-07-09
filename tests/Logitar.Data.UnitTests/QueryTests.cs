namespace Logitar.Data;

[Trait(Traits.Category, Categories.Unit)]
public class QueryTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct query.")]
  [InlineData("  SELECT * FROM [MyTable];  ")]
  [InlineData("SELECT * FROM [MyTable];", 2, 4, 6, 8)]
  public void Ctor_it_constructs_the_correct_query(string text, params object[] parameters)
  {
    Query query = new(text, parameters);
    Assert.Equal(text.Trim(), query.Text);
    Assert.Same(parameters, query.Parameters);
  }

  [Theory(DisplayName = "Ctor: it throws ArgumentException when text is null, empty, or only white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void Ctor_it_throws_ArgumentException_when_text_is_null_empty_or_only_white_space(string text)
  {
    var exception = Assert.Throws<ArgumentException>(() => new Query(text, Enumerable.Empty<object>()));
  }
}
