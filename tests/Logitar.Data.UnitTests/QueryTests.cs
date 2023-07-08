namespace Logitar.Data.UnitTests;

[Trait(Traits.Category, Categories.Unit)]
public class QueryTests
{
  [Theory(DisplayName = "Ctor: it constructs the correct query.")]
  [InlineData("SELECT * FROM [MyTable];")]
  public void Ctor_it_constructs_the_correct_query(string text)
  {
    Query query = new(text);
    Assert.Equal(text, query.Text);
  }
}
