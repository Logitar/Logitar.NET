namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class ApiResponseTests
{

  [Fact(DisplayName = "ctor: it should construct the correct API response.")]
  public void ctor_it_should_construct_the_correct_Api_response()
  {
    string contentLength = "Hello World!".Length.ToString();

    HttpResponseMessage response = new()
    {
      Version = new(1, 1),
      StatusCode = HttpStatusCode.OK,
      ReasonPhrase = "Success!",
    };
    response.Headers.Add("X-Content-Encoding", Encoding.UTF8.EncodingName);
    response.Headers.Add("X-Content-Length", contentLength);
    response.Headers.Add("X-Content-Type", MediaTypeNames.Text.Plain);

    string id = Guid.NewGuid().ToString();
    response.TrailingHeaders.Add("CorrelationId", id);

    ApiResponse result = new(response);
    Assert.Equal(response.Version, result.Version);
    Assert.Equal(response.StatusCode, result.Status.Value);
    Assert.Equal(response.ReasonPhrase, result.ReasonPhrase);

    Assert.Equal(3, result.Headers.Count);
    Assert.Contains(result.Headers, h => h.Name == "X-Content-Encoding" && h.Values.Single() == Encoding.UTF8.EncodingName);
    Assert.Contains(result.Headers, h => h.Name == "X-Content-Length" && h.Values.Single() == contentLength);
    Assert.Contains(result.Headers, h => h.Name == "X-Content-Type" && h.Values.Single() == MediaTypeNames.Text.Plain);

    HttpHeader header = Assert.Single(result.TrailingHeaders);
    Assert.Equal("CorrelationId", header.Name);
    Assert.Equal(id, header.Values.Single());
  }

  [Fact(DisplayName = "It should be serializable and deserializable.")]
  public void It_should_be_serializable_and_deserializable()
  {
    ApiResponse response = new()
    {
      Version = new(1, 1),
      Status = new(HttpStatusCode.BadRequest),
      ReasonPhrase = "Validation failed.",
    };
    response.Headers.Add(new("Content-Type", MediaTypeNames.Application.Json));
    response.TrailingHeaders.Add(new("CorrelationId", Guid.NewGuid().ToString()));

    string json = JsonSerializer.Serialize(response);
    ApiResponse? deserialized = JsonSerializer.Deserialize<ApiResponse>(json);
    Assert.NotNull(deserialized);
    Assert.Equal(response.Version, deserialized.Version);
    Assert.Equal(response.Status, deserialized.Status);
    Assert.Equal(response.ReasonPhrase, deserialized.ReasonPhrase);
    Assert.Equal(JsonSerializer.Serialize(response.Headers), JsonSerializer.Serialize(deserialized.Headers));
    Assert.Equal(JsonSerializer.Serialize(response.TrailingHeaders), JsonSerializer.Serialize(deserialized.TrailingHeaders));
  }
}
