namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class HttpHeaderTests
{
  [Theory(DisplayName = "ctor: it should construct the correct empty header.")]
  [InlineData("Content-Type")]
  public void ctor_it_should_construct_the_correct_empty_header(string name)
  {
    HttpHeader header = new(name);

    Assert.Equal(name, header.Name);
    Assert.Empty(header.Values);
  }

  [Theory(DisplayName = "ctor: it should construct the correct multiple-values header.")]
  [InlineData("Content-Type", "text/html", "application/json")]
  public void ctor_it_should_construct_the_correct_multiple_values_header(string name, params string[] values)
  {
    HttpHeader header = new(name, values);

    Assert.Equal(name, header.Name);
    Assert.Equal(values, header.Values);
  }

  [Theory(DisplayName = "ctor: it should construct the correct single-value header.")]
  [InlineData("Content-Type", "text/plain")]
  public void ctor_it_should_construct_the_correct_single_value_header(string name, string value)
  {
    HttpHeader header = new(name, value);

    Assert.Equal(name, header.Name);
    Assert.Equal(value, header.Values.Single());
  }
}
