namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class HttpStatusTests
{
  [Theory(DisplayName = "ctor: it should construct the correct status from a status code.")]
  [InlineData(HttpStatusCode.OK)]
  public void ctor_it_should_construct_the_correct_status_from_a_status_code(HttpStatusCode statusCode)
  {
    HttpStatus status = new(statusCode);
    Assert.Equal((int)statusCode, status.Code);
    Assert.Equal(statusCode.ToString(), status.Text);
    Assert.Equal(statusCode, status.Value);
    Assert.Equal((int)statusCode >= 200 && (int)statusCode <= 299, status.IsSuccess);
  }

  [Theory(DisplayName = "ctor: it should construct the correct status from an HttpResponseMessage.")]
  [InlineData(HttpStatusCode.InternalServerError)]
  public void ctor_it_should_construct_the_correct_status_from_an_HttpResponseMessage(HttpStatusCode statusCode)
  {
    HttpResponseMessage response = new(statusCode);
    HttpStatus status = new(response);
    Assert.Equal((int)statusCode, status.Code);
    Assert.Equal(statusCode.ToString(), status.Text);
    Assert.Equal(statusCode, status.Value);
    Assert.Equal(response.IsSuccessStatusCode, status.IsSuccess);
  }
}
