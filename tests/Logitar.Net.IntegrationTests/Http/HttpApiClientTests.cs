using Bogus;
using Logitar.Demo.Models.Request;
using Logitar.Net.Http.Settings;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Integration)]
public class HttpApiClientTests : IDisposable
{
  private const string BaseUrl = "http://localhost:22277";
  private const string Path = "/request";
  private static string RequestUrl => new UriBuilder(BaseUrl) { Path = Path }.ToString();

  private static readonly CancellationToken _cancellationToken = default;
  private static readonly Uri _requestUri = new(Path, UriKind.Relative);
  private static readonly JsonSerializerOptions _serializerOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

  private readonly Faker _faker = new();
  private readonly Person _person;

  private readonly HttpApiSettings _settings;
  private readonly HttpApiClient _client;

  public HttpApiClientTests()
  {
    _person = new(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth);

    _settings = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Basic",
        Credentials = "YWRtaW46UEBzJFcwckQ="
      },
      BaseUri = new Uri(BaseUrl, UriKind.Absolute)
    };
    _settings.Headers.Add(new HttpHeader("X-Test-Class", nameof(HttpApiClientTests)));

    _client = new(_settings);
  }

  public void Dispose()
  {
    _client.Dispose();
    GC.SuppressFinalize(this);
  }

  [Fact(DisplayName = "SendAsync: it should send a request with a body and a cancellation token.")]
  public async Task SendAsync_it_should_send_a_request_with_a_body_and_a_cancellation_token()
  {
    HttpMethod method = HttpMethod.Post;
    JsonContent requestContent = JsonContent.Create(_person);
    HttpApiResponse response = await _client.SendAsync(method, _requestUri, requestContent, _cancellationToken);

    AssertRequest(method, _person, response);
  }

  [Fact(DisplayName = "SendAsync: it should send a request with a body and a request context.")]
  public async Task SendAsync_it_should_send_a_request_with_a_body_and_a_request_context()
  {
    HttpRequestContext context = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Bearer",
        Credentials = Guid.NewGuid().ToString()
      }
    };

    HttpMethod method = HttpMethod.Post;
    JsonContent requestContent = JsonContent.Create(_person);
    HttpApiResponse response = await _client.SendAsync(method, _requestUri, requestContent, context);

    AssertRequest(method, _person, response, context.Authorization);
  }

  [Fact(DisplayName = "SendAsync: it should send a simple request with a cancellation token.")]
  public async Task SendAsync_it_should_send_a_simple_request_with_a_cancellation_token()
  {
    HttpMethod method = HttpMethod.Get;
    HttpApiResponse response = await _client.SendAsync(method, _requestUri, _cancellationToken);

    AssertRequest(method, response);
  }

  [Fact(DisplayName = "SendAsync: it should send a simple request with a request context.")]
  public async Task SendAsync_it_should_send_a_simple_request_with_a_request_context()
  {
    HttpRequestContext context = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Bearer",
        Credentials = Guid.NewGuid().ToString()
      }
    };

    HttpMethod method = HttpMethod.Delete;
    HttpApiResponse response = await _client.SendAsync(method, _requestUri, context);

    AssertRequest(method, response, context.Authorization);
  }

  private void AssertRequest(HttpMethod method, HttpApiResponse response)
  {
    AssertRequest(method, requestContent: null, response);
  }
  private void AssertRequest(HttpMethod method, HttpApiResponse response, IAuthorizationSettings? authorizationSettings)
  {
    AssertRequest(method, requestContent: null, response, authorizationSettings);
  }
  private void AssertRequest(HttpMethod method, object? requestContent, HttpApiResponse response)
  {
    AssertRequest(method, requestContent, response, authorizationSettings: null);
  }
  private void AssertRequest(HttpMethod method, object? requestContent, HttpApiResponse response, IAuthorizationSettings? authorizationSettings)
  {
    Assert.Equal(HttpStatusCode.OK, response.Status.Value);

    Assert.NotNull(response.ContentText);
    HttpRequestModel? request = JsonSerializer.Deserialize<HttpRequestModel>(response.ContentText, _serializerOptions);
    Assert.NotNull(request);

    if (requestContent == null)
    {
      Assert.Null(request.Content);
    }
    else
    {
      string json = JsonSerializer.Serialize(requestContent, _serializerOptions);
      Assert.Equal(json, request.Content);
    }

    Assert.Equal(method.Method, request.Method);

    Assert.Equal(RequestUrl, request.Url);

    authorizationSettings ??= _settings.Authorization;
    if (authorizationSettings != null)
    {
      Assert.Contains(request.Headers, h => h.Name == "Authorization" && h.Values.Single() == $"{authorizationSettings.Scheme} {authorizationSettings.Credentials}");
    }

    foreach (HttpHeader header in _settings.Headers)
    {
      Assert.Contains(request.Headers, h => h.Name == header.Name && h.Values.SequenceEqual(header.Values));
    }
  }
}
