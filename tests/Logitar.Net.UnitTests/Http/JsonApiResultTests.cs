using Bogus;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class JsonApiResultTests
{
  private static readonly JsonSerializerOptions _serializerOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

  private readonly Faker _faker = new();
  private readonly Person _person;

  public JsonApiResultTests()
  {
    _person = new(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth);
  }

  [Fact(DisplayName = "ctor: it should construct the correct result from an HttpResponseMessage.")]
  public void ctor_it_should_construct_the_correct_result_from_an_HttpResponseMessage()
  {
    HttpResponseMessage response = new(HttpStatusCode.Created)
    {
      Version = new(1, 1),
      ReasonPhrase = "Person saved."
    };
    response.Headers.Add("Location", "Jsons://www.francispion.ca/people/fpion");
    response.TrailingHeaders.Add("Timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

    JsonApiResult result = new(response);

    AssertResponse(response, result);
  }

  [Fact(DisplayName = "Deserialize: it should deserialize the correct value.")]
  public async Task Deserialize_it_should_deserialize_the_correct_value()
  {
    HttpResponseMessage response = new(HttpStatusCode.OK)
    {
      Content = JsonContent.Create(_person)
    };

    JsonApiResult result = await JsonApiResult.FromResponseAsync(response);

    Person? deserialized = result.Deserialize<Person>(_serializerOptions);
    Assert.NotNull(deserialized);
    Assert.Equal(_person, deserialized);
  }

  [Fact(DisplayName = "Deserialize: it should return default when JsonContent is null.")]
  public void Deserialize_it_should_return_default_when_JsonContent_is_null()
  {
    JsonApiResult result = new()
    {
      JsonContent = null
    };
    Assert.False(result.Deserialize<bool>());
  }

  [Fact(DisplayName = "FromResponseAsync: it should construct the correct result from an HttpResponseMessage.")]
  public async Task FromResponseAsync_it_should_contruct_the_correct_result_from_an_HttpResponseMessage()
  {
    HttpResponseMessage response = new(HttpStatusCode.Created)
    {
      Version = new(1, 1),
      ReasonPhrase = "Person saved.",
      Content = JsonContent.Create(new Person(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth))
    };

    JsonApiResult result = await JsonApiResult.FromResponseAsync(response);

    AssertResponse(response, result, _person);
  }

  private static void AssertResponse(HttpResponseMessage response, JsonApiResult result, object? content = null)
  {
    Assert.Equal(response.Version, result.Version);
    Assert.Equal(response.StatusCode, result.Status.Value);
    Assert.Equal(response.ReasonPhrase, result.ReasonPhrase);

    foreach (KeyValuePair<string, IEnumerable<string>> header in response.Headers)
    {
      Assert.Contains(result.Headers, h => h.Name == header.Key && h.Values.SequenceEqual(header.Value));
    }

    foreach (KeyValuePair<string, IEnumerable<string>> header in response.TrailingHeaders)
    {
      Assert.Contains(result.TrailingHeaders, h => h.Name == header.Key && h.Values.SequenceEqual(header.Value));
    }

    if (content == null)
    {
      Assert.Null(result.JsonContent);
    }
    else
    {
      string jsonContent = JsonSerializer.Serialize(content, _serializerOptions);
      Assert.Equal(jsonContent, result.JsonContent);
    }
  }
}
