namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class HttpApiResponseTests
{
  [Fact(DisplayName = "ctor: it should construct the correct API response.")]
  public void ctor_it_should_construct_the_correct_Api_response()
  {
    HttpResponseMessage response = new()
    {
      Content = new StringContent("Hello World!")
    };

    HttpApiResponse result = new(response);
    Assert.Same(response.Content, result.Content);
  }

  [Fact(DisplayName = "It should be serializable and deserializable.")]
  public void It_should_be_serializable_and_deserializable()
  {
    HttpResponseMessage response = new()
    {
      Content = new StringContent("Hello World!")
    };

    string json = JsonSerializer.Serialize(response);
    HttpApiResponse? deserialized = JsonSerializer.Deserialize<HttpApiResponse>(json);
    Assert.NotNull(deserialized);
    Assert.Null(deserialized.Content);
  }
}
