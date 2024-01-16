using System.Net;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class ApiStatusTests
{
  [Theory(DisplayName = "ctor: it should construct the correct API status from a status code.")]
  [InlineData(HttpStatusCode.OK)]
  [InlineData(HttpStatusCode.BadRequest)]
  public void ctor_it_should_construct_the_correct_Api_status_from_a_status_code(HttpStatusCode statusCode)
  {
    ApiStatus status = new(statusCode);

    Assert.Equal((int)statusCode, status.Code);
    Assert.Equal(statusCode.ToString(), status.Text);
    Assert.Equal(statusCode, status.Value);
    Assert.Equal(status.Code >= 200 && status.Code <= 299, status.IsSuccess);
  }

  [Theory(DisplayName = "ctor: it should construct the correct API status from a status code and a success indicator.")]
  [InlineData(HttpStatusCode.NoContent, false)]
  [InlineData(HttpStatusCode.Forbidden, true)]
  public void ctor_it_should_construct_the_correct_Api_status_from_a_status_code_and_a_success_indicating(HttpStatusCode statusCode, bool isSuccess)
  {
    ApiStatus status = new(statusCode, isSuccess);

    Assert.Equal((int)statusCode, status.Code);
    Assert.Equal(statusCode.ToString(), status.Text);
    Assert.Equal(statusCode, status.Value);
    Assert.Equal(isSuccess, status.IsSuccess);
  }

  [Theory(DisplayName = "ctor: it should construct the correct Api status from an Http response message.")]
  [InlineData(HttpStatusCode.Created)]
  [InlineData(HttpStatusCode.Conflict)]
  public void ctor_it_should_construct_the_correct_Api_status_from_an_Http_response_message(HttpStatusCode statusCode)
  {
    HttpResponseMessage response = new(statusCode);

    ApiStatus status = new(response);

    Assert.Equal((int)response.StatusCode, status.Code);
    Assert.Equal(response.StatusCode.ToString(), status.Text);
    Assert.Equal(response.StatusCode, status.Value);
    Assert.Equal(response.IsSuccessStatusCode, status.IsSuccess);
  }
}
