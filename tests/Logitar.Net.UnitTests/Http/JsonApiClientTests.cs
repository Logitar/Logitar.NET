using Bogus;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class JsonApiClientTests
{
  private static readonly JsonSerializerOptions _serializerOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

  private readonly JsonApiClientMock _client = new();
  private readonly Faker _faker = new();

  [Fact(DisplayName = "BuildResultAsync: it should build the correct result.")]
  public async Task BuildResultAsync_it_should_build_the_correct_result()
  {
    Person person = new(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth);

    HttpResponseMessage response = new(HttpStatusCode.NoContent)
    {
      Version = new(1, 1),
      ReasonPhrase = "Operation completed.",
      Content = JsonContent.Create(person)
    };
    response.Headers.Add("Timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

    CancellationToken cancellationToken = default;
    JsonApiResult result = await _client.BuildResultAsync(response, cancellationToken);

    Assert.Equal(response.Version, result.Version);
    Assert.Equal(response.StatusCode, result.Status.Value);
    Assert.Equal(response.ReasonPhrase, result.ReasonPhrase);

    foreach (KeyValuePair<string, IEnumerable<string>> header in response.Headers)
    {
      Assert.Contains(result.Headers, h => h.Name == header.Key && h.Values.SequenceEqual(header.Value));
    }

    string jsonContent = JsonSerializer.Serialize(person, _serializerOptions);
    Assert.Equal(jsonContent, result.JsonContent);
  }

  [Fact(DisplayName = "ctor: it should construct the correct JsonApiClient.")]
  public void ctor_it_should_construct_the_correct_JsonApiClient()
  {
    HttpApiSettings settings = new()
    {
      BaseUri = new("https://www.tests.com"),
      ThrowOnFailure = false
    };

    JsonApiClientMock client;

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
