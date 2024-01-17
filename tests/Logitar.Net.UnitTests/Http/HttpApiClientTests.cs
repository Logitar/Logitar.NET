using Bogus;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class HttpApiClientTests
{
  private readonly HttpApiClientMock _client = new();
  private readonly Faker _faker = new();

  [Fact(DisplayName = "BuildRequestAsync: it should build the correct HttpRequestMessage.")]
  public async Task BuildRequestAsync_it_should_build_the_correct_HttpRequestMessage()
  {
    HttpRequestParameters parameters = new()
    {
      Method = HttpMethod.Post,
      Uri = new("/people", UriKind.Relative),
      Content = JsonContent.Create(new Person(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth)),
      Authorization = HttpAuthorization.Bearer(Guid.NewGuid().ToString())
    };
    parameters.Headers.Add(new HttpHeader("ClientId", Guid.NewGuid().ToString()));

    CancellationToken cancellationToken = default;
    HttpRequestMessage request = await _client.BuildRequestAsync(parameters, cancellationToken);

    Assert.Equal(parameters.Method, request.Method);
    Assert.Equal(parameters.Uri, request.RequestUri);
    Assert.Same(parameters.Content, request.Content);

    foreach (HttpHeader header in parameters.Headers)
    {
      Assert.Contains(request.Headers, h => h.Key == header.Name && h.Value.SequenceEqual(header.Values));
    }

    Assert.NotNull(request.Headers.Authorization);
    Assert.Equal(parameters.Authorization.Scheme, request.Headers.Authorization.Scheme);
    Assert.Equal(parameters.Authorization.Credentials, request.Headers.Authorization.Parameter);
  }

  [Fact(DisplayName = "BuildResultAsync: it should build the correct result.")]
  public async Task BuildResultAsync_it_should_build_the_correct_result()
  {
    HttpResponseMessage response = new(HttpStatusCode.NoContent)
    {
      Version = new(1, 1),
      ReasonPhrase = "Operation completed."
    };
    response.Headers.Add("Timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

    CancellationToken cancellationToken = default;
    HttpApiResult result = await _client.BuildResultAsync(response, cancellationToken);

    Assert.Equal(response.Version, result.Version);
    Assert.Equal(response.StatusCode, result.Status.Value);
    Assert.Equal(response.ReasonPhrase, result.ReasonPhrase);

    foreach (KeyValuePair<string, IEnumerable<string>> header in response.Headers)
    {
      Assert.Contains(result.Headers, h => h.Name == header.Key && h.Values.SequenceEqual(header.Value));
    }
  }

  [Fact(DisplayName = "ctor: it should construct the correct HttpApiClient.")]
  public void ctor_it_should_construct_the_correct_HttpApiClient()
  {
    HttpApiSettings settings = new()
    {
      BaseUri = new("https://www.tests.com"),
      ThrowOnFailure = false
    };

    HttpApiClientMock client;

    client = new();
    Assert.True(client.DisposeClient);
    Assert.True(client.ThrowOnFailure);
    Assert.Null(client.Client.BaseAddress);

    client = new(settings);
    Assert.True(client.DisposeClient);
    Assert.Equal(settings.ThrowOnFailure, client.ThrowOnFailure);
    Assert.Equal(settings.BaseUri, client.Client.BaseAddress);

    using HttpClient httpClient = new();

    client = new(httpClient);
    Assert.False(client.DisposeClient);
    Assert.True(client.ThrowOnFailure);
    Assert.Same(httpClient, client.Client);
    Assert.Null(client.Client.BaseAddress);

    client = new(httpClient, settings);
    Assert.False(client.DisposeClient);
    Assert.Equal(settings.ThrowOnFailure, client.ThrowOnFailure);
    Assert.Same(httpClient, client.Client);
    Assert.Equal(settings.BaseUri, client.Client.BaseAddress);
  }
}
