using Bogus;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Integration)]
public class HttpApiClientExtensionsTests : IDisposable
{
  private readonly CancellationToken _cancellationToken = default;
  private readonly Faker _faker = new();
  private readonly Person _person;
  private readonly Uri _uri = new("/test", UriKind.Relative);

  private readonly HttpApiSettings _settings;
  private readonly HttpApiClientMock _client;

  public HttpApiClientExtensionsTests()
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

  [Fact(DisplayName = "DeleteAsync: it should send the correct DELETE request with options.")]
  public async Task DeleteAsync_it_should_send_the_correct_Delete_request_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Delete(_uri);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    HttpApiResult result = await _client.DeleteAsync(parameters.Uri, parameters, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "DeleteAsync: it should send the correct DELETE request without options.")]
  public async Task DeleteAsync_it_should_send_the_correct_Delete_request_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Delete(_uri);

    HttpApiResult result = await _client.DeleteAsync(parameters.Uri, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "GetAsync: it should send the correct GET request with options.")]
  public async Task GetAsync_it_should_send_the_correct_Get_request_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Get(_uri);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    HttpApiResult result = await _client.GetAsync(parameters.Uri, parameters, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "GetAsync: it should send the correct GET request without options.")]
  public async Task GetAsync_it_should_send_the_correct_Get_request_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Get(_uri);

    HttpApiResult result = await _client.GetAsync(parameters.Uri, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "PatchAsync: it should send the correct PATCH request with options.")]
  public async Task PatchAsync_it_should_send_the_correct_Patch_request_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Patch(_uri);
    parameters.Content = JsonContent.Create(_person);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    HttpApiResult result = await _client.PatchAsync(parameters.Uri, parameters, _cancellationToken);
    result.AssertIsValid(parameters, _person, _settings);
  }

  [Fact(DisplayName = "PatchAsync: it should send the correct PATCH request without options.")]
  public async Task PatchAsync_it_should_send_the_correct_Patch_request_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Patch(_uri);

    HttpApiResult result = await _client.PatchAsync(parameters.Uri, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "PostAsync: it should send the correct POST request with options.")]
  public async Task PostAsync_it_should_send_the_correct_Post_request_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Post(_uri);
    parameters.Content = JsonContent.Create(_person);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    HttpApiResult result = await _client.PostAsync(parameters.Uri, parameters, _cancellationToken);
    result.AssertIsValid(parameters, _person, _settings);
  }

  [Fact(DisplayName = "PostAsync: it should send the correct POST request without options.")]
  public async Task PostAsync_it_should_send_the_correct_Post_request_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Post(_uri);

    HttpApiResult result = await _client.PostAsync(parameters.Uri, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "PutAsync: it should send the correct PUT request with options.")]
  public async Task PutAsync_it_should_send_the_correct_Put_request_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Put(_uri);
    parameters.Content = JsonContent.Create(_person);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    HttpApiResult result = await _client.PutAsync(parameters.Uri, parameters, _cancellationToken);
    result.AssertIsValid(parameters, _person, _settings);
  }

  [Fact(DisplayName = "PutAsync: it should send the correct PUT request without options.")]
  public async Task PutAsync_it_should_send_the_correct_Put_request_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Put(_uri);

    HttpApiResult result = await _client.PutAsync(parameters.Uri, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }
}
