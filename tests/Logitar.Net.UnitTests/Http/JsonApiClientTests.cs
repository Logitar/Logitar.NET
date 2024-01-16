using Logitar.Net.Http.Settings;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class JsonApiClientTests
{
  private readonly JsonApiSettings _settings;

  public JsonApiClientTests()
  {
    _settings = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Basic",
        Credentials = "YWRtaW46UEBzJFcwckQ="
      },
      BaseUri = new Uri("https://api.test.com/", UriKind.Absolute),
      ThrowOnFailure = true,
      SerializerOptions = new()
      {
        PropertyNameCaseInsensitive = true
      }
    };
    _settings.Headers.Add(new HttpHeader("X-Realm", "test"));
  }

  [Fact(DisplayName = "ctor: it should construct a JSON API client with settings.")]
  public void ctor_it_should_construct_a_Json_Api_client_with_settings()
  {
    FakeJsonApiClient client = new(_settings);
    Assert.True(client.DisposeClient);
    Assert.Same(_settings.SerializerOptions, client.SerializerOptions);
    Assert.Equal(_settings.ThrowOnFailure, client.ThrowOnFailure);
    Assert.Equal(_settings.BaseUri, client.Client.BaseAddress);

    Assert.NotNull(_settings.Authorization);
    Assert.NotNull(client.Client.DefaultRequestHeaders.Authorization);
    Assert.Equal(_settings.Authorization.Scheme, client.Client.DefaultRequestHeaders.Authorization.Scheme);
    Assert.Equal(_settings.Authorization.Credentials, client.Client.DefaultRequestHeaders.Authorization.Parameter);

    KeyValuePair<string, IEnumerable<string>> header = client.Client.DefaultRequestHeaders.Single(h => h.Key != "Authorization");
    Assert.Equal(_settings.Headers.Single().Name, header.Key);
    Assert.Equal(_settings.Headers.Single().Values.Single(), header.Value.Single());
  }

  [Fact(DisplayName = "ctor: it should construct a JSON API client with settings and client.")]
  public void ctor_it_should_construct_a_Json_Api_client_with_settings_and_client()
  {
    HttpClient httpClient = new();
    FakeJsonApiClient client = new(httpClient, _settings);
    Assert.False(client.DisposeClient);
    Assert.Same(httpClient, client.Client);
    Assert.Same(_settings.SerializerOptions, client.SerializerOptions);
    Assert.Equal(_settings.ThrowOnFailure, client.ThrowOnFailure);
    Assert.Equal(_settings.BaseUri, client.Client.BaseAddress);

    Assert.NotNull(_settings.Authorization);
    Assert.NotNull(client.Client.DefaultRequestHeaders.Authorization);
    Assert.Equal(_settings.Authorization.Scheme, client.Client.DefaultRequestHeaders.Authorization.Scheme);
    Assert.Equal(_settings.Authorization.Credentials, client.Client.DefaultRequestHeaders.Authorization.Parameter);

    KeyValuePair<string, IEnumerable<string>> header = client.Client.DefaultRequestHeaders.Single(h => h.Key != "Authorization");
    Assert.Equal(_settings.Headers.Single().Name, header.Key);
    Assert.Equal(_settings.Headers.Single().Values.Single(), header.Value.Single());
  }

  [Fact(DisplayName = "ctor: it should construct a JSON API client without settings.")]
  public void ctor_it_should_construct_a_Json_Api_client_without_settings()
  {
    HttpClient httpClient = new();
    FakeJsonApiClient client = new(httpClient);
    Assert.Same(httpClient, client.Client);
    Assert.False(client.DisposeClient);
  }

  [Fact(DisplayName = "ctor: it should construct the default JSON API client.")]
  public void ctor_it_should_construct_the_default_Json_Api_client()
  {
    FakeJsonApiClient client = new();
    Assert.True(client.DisposeClient);
  }
}
