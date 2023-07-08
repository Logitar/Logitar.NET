namespace Logitar.Data.UnitTests;

[Trait(Traits.Category, Categories.Unit)]
public class QueryTests
{
  [Theory(DisplayName = "Ctor: it builds correctly.")]
  [InlineData("SELECT * FROM [MyTable];")]
  public void Ctor_it_builds_correctly(string text)
  {
    Query query = new(text);
    Assert.Equal(text, query.Text);
  }
}
