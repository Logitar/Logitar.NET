using Bogus;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class JsonApiResponseTests
{
  private readonly Faker _faker = new();

  [Fact(DisplayName = "ctor: it should construct the correct API response.")]
  public async Task ctor_it_should_construct_the_correct_Api_response()
  {
    Person person = new(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth);
    HttpResponseMessage response = new()
    {
      Content = JsonContent.Create(person)
    };

    JsonSerializerOptions serializerOptions = new()
    {
      PropertyNameCaseInsensitive = true
    };
    JsonApiResponse<Person> result = await JsonApiResponse<Person>.FromResponseAsync(response, serializerOptions);
    Assert.Equal(JsonSerializer.Serialize(person), result.ContentText);
    Assert.NotNull(result.Value);
    Assert.Equal(person, result.Value);
  }

  [Fact(DisplayName = "It should be serializable and deserializable.")]
  public void It_should_be_serializable_and_deserializable()
  {
    Person person = new(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth);
    JsonApiResponse<Person> response = new()
    {
      Value = person
    };

    string json = JsonSerializer.Serialize(response);
    JsonApiResponse<Person>? deserialized = JsonSerializer.Deserialize<JsonApiResponse<Person>>(json);
    Assert.NotNull(deserialized);
    Assert.Equal(JsonSerializer.Serialize(person), deserialized.ContentText);
    Assert.Equal(person, deserialized.Value);
  }
}
