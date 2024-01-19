namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class HttpApiResultTests
{
  [Fact(DisplayName = "ctor: it should construct the correct result from an HttpResponseMessage.")]
  public void ctor_it_should_construct_the_correct_result_from_an_HttpResponseMessage()
  {
    HttpResponseMessage response = new(HttpStatusCode.Created)
    {
      Version = new(1, 1),
      ReasonPhrase = "Person saved."
    };
    response.Headers.Add("Location", "https://www.francispion.ca/people/fpion");
    response.TrailingHeaders.Add("Timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());

    HttpApiResult result = new(response);
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
  }
}
