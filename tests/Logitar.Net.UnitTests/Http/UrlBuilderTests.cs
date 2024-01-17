namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class UrlBuilderTests
{
  private readonly UrlBuilder _builder = new();

  [Theory(DisplayName = "AddQuery: it should add a query parameter value.")]
  [InlineData("id", "123")]
  public void AddQuery_it_should_add_a_query_parameter_value(string key, string value)
  {
    _builder.AddQuery(key, value);
    Assert.Equal([value], _builder.Query[key]);

    string altValue = new(value.Reverse().ToArray());
    Assert.NotEqual(altValue, value);

    _builder.AddQuery(key, altValue);
    Assert.Equal([value, altValue], _builder.Query[key]);
  }

  [Theory(DisplayName = "AddQuery: it should add query parameter values.")]
  [InlineData("id", "123", "456", "789")]
  public void AddQuery_it_should_add_query_parameter_values(string key, params string[] values)
  {
    string initialValue = Guid.NewGuid().ToString();
    Assert.DoesNotContain(initialValue, values);

    _builder.AddQuery(key, initialValue);
    Assert.Equal([initialValue], _builder.Query[key]);

    _builder.AddQuery(key, values);
    Assert.Equal(new[] { initialValue }.Concat(values), _builder.Query[key]);
  }

  //[Fact(DisplayName = "BuildUri: it should build the correct URI.")]
  //public void BuildUri_it_should_build_the_correct_Uri()
  //{
  //  string id = Guid.NewGuid().ToString();
  //  string expected = $"https://www.test.com/people/{id}?include=identifiers&include=sessions&fields=first_name,last_name";

  //  _builder.SetUrl("https://www.test.com/people/{id}");
  //  _builder.SetParameter("id", id);
  //  _builder.SetQuery("include", "identifiers");
  //  _builder.AddQuery("include", "sessions");
  //  _builder.SetQuery("fields", "first_name,last_name");
  //  Uri uri = _builder.BuildUri();

  //  Assert.Equal(expected, uri.ToString());
  //}

  [Theory(DisplayName = "SetParameter: it should set an URL parameter.")]
  [InlineData("id", "123")]
  public void SetParameter_it_should_set_an_Url_parameter(string key, string value)
  {
    _builder.SetParameter(key, value);
    Assert.Equal(value, _builder.Parameters[key]);
  }

  [Theory(DisplayName = "SetQuery: it should set a query parameter value.")]
  [InlineData("id", "123")]
  public void SetQuery_it_should_set_a_query_parameter_value(string key, string value)
  {
    string altValue = new(value.Reverse().ToArray());
    Assert.NotEqual(altValue, value);

    _builder.SetQuery(key, altValue);
    Assert.Equal([altValue], _builder.Query[key]);

    _builder.SetQuery(key, value);
    Assert.Equal([value], _builder.Query[key]);
  }

  [Theory(DisplayName = "SetQuery: it should set query parameter values.")]
  [InlineData("id", "123", "456", "789")]
  public void SetQuery_it_should_set_query_parameter_values(string key, params string[] values)
  {
    string initialValue = Guid.NewGuid().ToString();
    Assert.DoesNotContain(initialValue, values);

    _builder.SetQuery(key, initialValue);
    Assert.Equal([initialValue], _builder.Query[key]);

    _builder.SetQuery(key, values);
    Assert.Equal(values, _builder.Query[key]);
  }
}
