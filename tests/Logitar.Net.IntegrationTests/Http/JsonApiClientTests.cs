using Bogus;
using Logitar.Demo.Models.Request;
using Logitar.Net.Http.Settings;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Integration)]
public class JsonApiClientTests : IDisposable
{
  private const string BaseUrl = "http://localhost:22277";
  private const string Path = "/request";
  private static string RequestUrl => new UriBuilder(BaseUrl) { Path = Path }.ToString();

  private static readonly CancellationToken _cancellationToken = default;
  private static readonly Uri _requestUri = new(Path, UriKind.Relative);

  private readonly Faker _faker = new();
  private readonly Person _person;

  private readonly JsonApiSettings _settings;
  private readonly JsonApiClient _client;

  public JsonApiClientTests()
  {
    _person = new(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth);

    _settings = new()
    {
      Authorization = new AuthorizationSettings
      {
        Scheme = "Basic",
        Credentials = "YWRtaW46UEBzJFcwckQ="
      },
      BaseUri = new Uri(BaseUrl, UriKind.Absolute),
      SerializerOptions = new()
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      }
    };
    _settings.Headers.Add(new HttpHeader("X-Test-Class", nameof(JsonApiClientTests)));

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
    JsonApiResponse<HttpRequestModel> response = await _client.SendAsync<HttpRequestModel>(method, _requestUri, _person, _cancellationToken);

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
    JsonApiResponse<HttpRequestModel> response = await _client.SendAsync<HttpRequestModel>(method, _requestUri, _person, context);

    AssertRequest(method, _person, response, context.Authorization);
  }

  [Fact(DisplayName = "SendAsync: it should send a simple request with a cancellation token.")]
  public async Task SendAsync_it_should_send_a_simple_request_with_a_cancellation_token()
  {
    HttpMethod method = HttpMethod.Get;
    JsonApiResponse<HttpRequestModel> response = await _client.SendAsync<HttpRequestModel>(method, _requestUri, _cancellationToken);

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
    JsonApiResponse<HttpRequestModel> response = await _client.SendAsync<HttpRequestModel>(method, _requestUri, context);

    AssertRequest(method, response, context.Authorization);
  }

  private void AssertRequest(HttpMethod method, JsonApiResponse<HttpRequestModel> response)
  {
    AssertRequest(method, requestContent: null, response);
  }
  private void AssertRequest(HttpMethod method, JsonApiResponse<HttpRequestModel> response, IAuthorizationSettings? authorizationSettings)
  {
    AssertRequest(method, requestContent: null, response, authorizationSettings);
  }
  private void AssertRequest(HttpMethod method, object? requestContent, JsonApiResponse<HttpRequestModel> response)
  {
    AssertRequest(method, requestContent, response, authorizationSettings: null);
  }
  private void AssertRequest(HttpMethod method, object? requestContent, JsonApiResponse<HttpRequestModel> response, IAuthorizationSettings? authorizationSettings)
  {
    Assert.Equal(HttpStatusCode.OK, response.Status.Value);

    HttpRequestModel? request = response.Value;
    Assert.NotNull(request);

    if (requestContent == null)
    {
      Assert.Null(request.Content);
    }
    else
    {
      string json = JsonSerializer.Serialize(requestContent, _settings.SerializerOptions);
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
