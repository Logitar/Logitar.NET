using Bogus;
using Logitar.Demo.Models.Test;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Integration)]
public class JsonApiClientExtensionsTests : IDisposable
{
  private readonly CancellationToken _cancellationToken = default;
  private readonly Faker _faker = new();
  private readonly Person _person;
  private readonly Uri _uri = new("/test", UriKind.Relative);

  private readonly HttpApiSettings _settings;
  private readonly JsonApiClientMock _client;

  public JsonApiClientExtensionsTests()
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

  [Fact(DisplayName = "DeleteAsync: it should deserialize the correct DELETE result with options.")]
  public async Task DeleteAsync_it_should_deserialize_the_correct_Delete_result_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Delete(_uri);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    HttpRequestModel? request = await _client.DeleteAsync<HttpRequestModel>(parameters.Uri, parameters.ToJsonOptions(), _cancellationToken);
    Assert.NotNull(request);
    request.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "DeleteAsync: it should deserialize the correct DELETE result without options.")]
  public async Task DeleteAsync_it_should_deserialize_the_correct_Delete_result_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Delete(_uri);

    JsonRequestModel? request = await _client.DeleteAsync<JsonRequestModel>(parameters.Uri, _cancellationToken);
    Assert.NotNull(request);
    request.ToHttpModel().AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "DeleteAsync: it should send the correct DELETE request with options.")]
  public async Task DeleteAsync_it_should_send_the_correct_Delete_request_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Delete(_uri);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    JsonApiResult result = await _client.DeleteAsync(parameters.Uri, parameters.ToJsonOptions(), _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "DeleteAsync: it should send the correct DELETE request without options.")]
  public async Task DeleteAsync_it_should_send_the_correct_Delete_request_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Delete(_uri);

    JsonApiResult result = await _client.DeleteAsync(parameters.Uri, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "GetAsync: it should deserialize the correct GET result with options.")]
  public async Task GetAsync_it_should_deserialize_the_correct_Get_result_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Get(_uri);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    HttpRequestModel? request = await _client.GetAsync<HttpRequestModel>(parameters.Uri, parameters.ToJsonOptions(), _cancellationToken);
    Assert.NotNull(request);
    request.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "GetAsync: it should deserialize the correct GET result without options.")]
  public async Task GetAsync_it_should_deserialize_the_correct_Get_result_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Get(_uri);

    JsonRequestModel? request = await _client.GetAsync<JsonRequestModel>(parameters.Uri, _cancellationToken);
    Assert.NotNull(request);
    request.ToHttpModel().AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "GetAsync: it should send the correct GET request with options.")]
  public async Task GetAsync_it_should_send_the_correct_Get_request_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Get(_uri);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    JsonApiResult result = await _client.GetAsync(parameters.Uri, parameters.ToJsonOptions(), _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "GetAsync: it should send the correct GET request without options.")]
  public async Task GetAsync_it_should_send_the_correct_Get_request_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Get(_uri);

    JsonApiResult result = await _client.GetAsync(parameters.Uri, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "PatchAsync: it should deserialize the correct PATCH result with options.")]
  public async Task PatchAsync_it_should_deserialize_the_correct_Patch_result_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Patch(_uri);
    parameters.Content = JsonContent.Create(_person);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    HttpRequestModel? request = await _client.PatchAsync<HttpRequestModel>(parameters.Uri, parameters.ToJsonOptions(), _cancellationToken);
    Assert.NotNull(request);
    request.AssertIsValid(parameters, _person, _settings);
  }

  [Fact(DisplayName = "PatchAsync: it should deserialize the correct PATCH result without options.")]
  public async Task PatchAsync_it_should_deserialize_the_correct_Patch_result_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Patch(_uri);

    JsonRequestModel? request = await _client.PatchAsync<JsonRequestModel>(parameters.Uri, _cancellationToken);
    Assert.NotNull(request);
    request.ToHttpModel().AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "PatchAsync: it should send the correct PATCH request with options.")]
  public async Task PatchAsync_it_should_send_the_correct_Patch_request_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Patch(_uri);
    parameters.Content = JsonContent.Create(_person);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    JsonApiResult result = await _client.PatchAsync(parameters.Uri, parameters.ToJsonOptions(), _cancellationToken);
    result.AssertIsValid(parameters, _person, _settings);
  }

  [Fact(DisplayName = "PatchAsync: it should send the correct PATCH request without options.")]
  public async Task PatchAsync_it_should_send_the_correct_Patch_request_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Patch(_uri);

    JsonApiResult result = await _client.PatchAsync(parameters.Uri, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "PostAsync: it should deserialize the correct POST result with options.")]
  public async Task PostAsync_it_should_deserialize_the_correct_Post_result_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Post(_uri);
    parameters.Content = JsonContent.Create(_person);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    HttpRequestModel? request = await _client.PostAsync<HttpRequestModel>(parameters.Uri, parameters.ToJsonOptions(), _cancellationToken);
    Assert.NotNull(request);
    request.AssertIsValid(parameters, _person, _settings);
  }

  [Fact(DisplayName = "PostAsync: it should deserialize the correct POST result without options.")]
  public async Task PostAsync_it_should_deserialize_the_correct_Post_result_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Post(_uri);

    JsonRequestModel? request = await _client.PostAsync<JsonRequestModel>(parameters.Uri, _cancellationToken);
    Assert.NotNull(request);
    request.ToHttpModel().AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "PostAsync: it should send the correct POST request with options.")]
  public async Task PostAsync_it_should_send_the_correct_Post_request_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Post(_uri);
    parameters.Content = JsonContent.Create(_person);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    JsonApiResult result = await _client.PostAsync(parameters.Uri, parameters.ToJsonOptions(), _cancellationToken);
    result.AssertIsValid(parameters, _person, _settings);
  }

  [Fact(DisplayName = "PostAsync: it should send the correct POST request without options.")]
  public async Task PostAsync_it_should_send_the_correct_Post_request_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Post(_uri);

    JsonApiResult result = await _client.PostAsync(parameters.Uri, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "PutAsync: it should deserialize the correct PUT result with options.")]
  public async Task PutAsync_it_should_deserialize_the_correct_Put_result_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Put(_uri);
    parameters.Content = JsonContent.Create(_person);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    HttpRequestModel? request = await _client.PutAsync<HttpRequestModel>(parameters.Uri, parameters.ToJsonOptions(), _cancellationToken);
    Assert.NotNull(request);
    request.AssertIsValid(parameters, _person, _settings);
  }

  [Fact(DisplayName = "PutAsync: it should deserialize the correct PUT result without options.")]
  public async Task PutAsync_it_should_deserialize_the_correct_Put_result_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Put(_uri);

    JsonRequestModel? request = await _client.PutAsync<JsonRequestModel>(parameters.Uri, _cancellationToken);
    Assert.NotNull(request);
    request.ToHttpModel().AssertIsValid(parameters, _settings);
  }

  [Fact(DisplayName = "PutAsync: it should send the correct PUT request with options.")]
  public async Task PutAsync_it_should_send_the_correct_Put_request_with_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Put(_uri);
    parameters.Content = JsonContent.Create(_person);
    parameters.Headers.Add(new HttpHeader("ClientSecret", IntegrationTestHelper.GenerateSecret()));
    parameters.Authorization = HttpAuthorization.Bearer(IntegrationTestHelper.GenerateApiKey());

    JsonApiResult result = await _client.PutAsync(parameters.Uri, parameters.ToJsonOptions(), _cancellationToken);
    result.AssertIsValid(parameters, _person, _settings);
  }

  [Fact(DisplayName = "PutAsync: it should send the correct PUT request without options.")]
  public async Task PutAsync_it_should_send_the_correct_Put_request_without_options()
  {
    HttpRequestParameters parameters = HttpRequestParameters.Put(_uri);

    JsonApiResult result = await _client.PutAsync(parameters.Uri, _cancellationToken);
    result.AssertIsValid(parameters, _settings);
  }
}
