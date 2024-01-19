using Bogus;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Integration)]
public class HttpApiClientTests : IDisposable
{
  private readonly CancellationToken _cancellationToken = default;
  private readonly Faker _faker = new();
  private readonly Person _person;
  private readonly Uri _uri = new("/test", UriKind.Relative);

  private readonly HttpApiSettings _settings;
  private readonly HttpApiClientMock _client;

  public HttpApiClientTests()
  {
    _person = new(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth);

    _settings = new()
    {
      BaseUri = new Uri("http://localhost:48080", UriKind.Absolute),
      Authorization = new HttpAuthorization("admin", "P@s$W0rD")
    };
    _settings.Headers.Add(new HttpHeader("ClientId", Guid.NewGuid().ToString()));
    _client = new(_settings);
  }

  public void Dispose()
  {
    _client.Dispose();
    GC.SuppressFinalize(this);
  }

  [Fact(DisplayName = "SendAsync: it should send the correct request and build the correct result.")]
  public async Task SendAsync_it_should_send_the_correct_request_and_build_the_correct_result()
  {
    HttpRequestParameters parameters = new()
    {
      Method = HttpMethod.Post,
      Uri = _uri,
      Content = JsonContent.Create(_person)
    };
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    HttpApiResult result = await _client.SendAsync(parameters, _cancellationToken);
    result.AssertIsValid(parameters, _person, _settings);
  }

  [Fact(DisplayName = "SendAsync: it should send the correct request without content and default authorization.")]
  public async Task SendAsync_it_should_send_the_correct_request_without_content_and_default_authorization()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Get(_uri);

    HttpApiResult result = await _client.SendAsync(parameters, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }
}
