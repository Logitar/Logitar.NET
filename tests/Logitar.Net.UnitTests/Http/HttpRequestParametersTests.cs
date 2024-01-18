namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class HttpRequestParametersTests
{
  private static readonly Uri _uri = new("/people", UriKind.RelativeOrAbsolute);

  [Fact(DisplayName = "Apply: it should apply the options to the parameters.")]
  public void Apply_it_should_apply_the_options_to_the_parameters()
  {
    HttpRequestParameters parameters = new();
    Assert.Null(parameters.Content);
    Assert.Empty(parameters.Headers);
    Assert.Null(parameters.Authorization);

    HttpRequestOptions options = new()
    {
      Content = new StringContent("Hello World!", Encoding.ASCII, MediaTypeNames.Text.Plain),
      Authorization = HttpAuthorization.Bearer(Guid.NewGuid().ToString())
    };
    options.Headers.Add(new HttpHeader("ClientId", Guid.NewGuid().ToString()));

    parameters.Apply(options);
    Assert.Same(options.Content, parameters.Content);
    Assert.Equal(options.Headers, parameters.Headers);
    Assert.Same(options.Authorization, parameters.Authorization);
  }

  [Fact(DisplayName = "ctor: it should construct the correct request parameters.")]
  public void ctor_it_should_construct_the_correct_request_parameters()
  {
    HttpMethod method = HttpMethod.Post;
    HttpRequestParameters parameters = new(method, _uri);
    Assert.Equal(method, parameters.Method);
    Assert.Equal(_uri, parameters.Uri);
  }

  [Fact(DisplayName = "Delete: it should construct the correct request parameters.")]
  public void Delete_it_should_construct_the_correct_request_parameters()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Delete(_uri);
    Assert.Equal(HttpMethod.Delete, parameters.Method);
    Assert.Equal(_uri, parameters.Uri);
  }

  [Fact(DisplayName = "Get: it should construct the correct request parameters.")]
  public void Get_it_should_construct_the_correct_request_parameters()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Get(_uri);
    Assert.Equal(HttpMethod.Get, parameters.Method);
    Assert.Equal(_uri, parameters.Uri);
  }

  [Fact(DisplayName = "Patch: it should construct the correct request parameters.")]
  public void Patch_it_should_construct_the_correct_request_parameters()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Patch(_uri);
    Assert.Equal(HttpMethod.Patch, parameters.Method);
    Assert.Equal(_uri, parameters.Uri);
  }

  [Fact(DisplayName = "Post: it should construct the correct request parameters.")]
  public void Post_it_should_construct_the_correct_request_parameters()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Post(_uri);
    Assert.Equal(HttpMethod.Post, parameters.Method);
    Assert.Equal(_uri, parameters.Uri);
  }

  [Fact(DisplayName = "Put: it should construct the correct request parameters.")]
  public void Put_it_should_construct_the_correct_request_parameters()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Put(_uri);
    Assert.Equal(HttpMethod.Put, parameters.Method);
    Assert.Equal(_uri, parameters.Uri);
  }
}
