namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class HttpClientExtensionsTests : IDisposable
{
  private readonly HttpClient _client = new();

  public void Dispose()
  {
    _client.Dispose();
    GC.SuppressFinalize(this);
  }

  [Fact(DisplayName = "Apply: it should apply the default settings.")]
  public void Apply_it_should_apply_the_default_settings()
  {
    HttpApiSettings settings = new();

    _client.Apply(settings);
    Assert.Null(_client.BaseAddress);
    Assert.Empty(_client.DefaultRequestHeaders);
    Assert.Null(_client.DefaultRequestHeaders.Authorization);
  }

  [Fact(DisplayName = "Apply: it should apply the specified settings.")]
  public void Apply_it_should_apply_the_specified_settings()
  {
    HttpApiSettings settings = new()
    {
      BaseUri = new("https://auth.test.com"),
      Authorization = HttpAuthorization.Bearer(Guid.NewGuid().ToString())
    };
    settings.Headers.Add(new HttpHeader("ClientId", Guid.NewGuid().ToString()));

    _client.Apply(settings);
    Assert.Same(settings.BaseUri, _client.BaseAddress);

    foreach (HttpHeader header in settings.Headers)
    {
      Assert.Contains(_client.DefaultRequestHeaders, h => h.Key == header.Name && h.Value.SequenceEqual(header.Values));
    }

    Assert.NotNull(_client.DefaultRequestHeaders.Authorization);
    Assert.Equal(settings.Authorization.Scheme, _client.DefaultRequestHeaders.Authorization.Scheme);
    Assert.Equal(settings.Authorization.Credentials, _client.DefaultRequestHeaders.Authorization.Parameter);
  }
}
