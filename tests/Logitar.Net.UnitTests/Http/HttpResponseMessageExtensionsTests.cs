namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class HttpResponseMessageExtensionsTests
{
  [Fact(DisplayName = "ThrowOnFailure: it should not throw any exception if the response succeeded.")]
  public void ThrowOnFailure_it_should_not_throw_any_exception_if_the_response_succeeded()
  {
    HttpResponseMessage response = new(HttpStatusCode.NoContent);
    response.ThrowOnFailure();
  }

  [Fact(DisplayName = "ThrowOnFailure: it should not throw any exception with result if the response succeeded.")]
  public void ThrowOnFailure_it_should_not_throw_any_exception_with_result_if_the_response_succeeded()
  {
    HttpResponseMessage response = new(HttpStatusCode.Accepted);
    Guid result = Guid.NewGuid();
    response.ThrowOnFailure(result);
  }

  [Fact(DisplayName = "ThrowOnFailure: it should throw an HttpFailureException if the response failed.")]
  public void ThrowOnFailure_it_should_throw_an_HttpFailureException_if_the_response_failed()
  {
    HttpResponseMessage response = new(HttpStatusCode.Unauthorized);
    Assert.Throws<HttpFailureException>(response.ThrowOnFailure);
  }

  [Fact(DisplayName = "ThrowOnFailure: it should throw an HttpFailureException with result if the response failed.")]
  public void ThrowOnFailure_it_should_throw_an_HttpFailureException_with_result_if_the_response_failed()
  {
    HttpResponseMessage response = new(HttpStatusCode.Forbidden);
    Guid result = Guid.NewGuid();
    var exception = Assert.Throws<HttpFailureException<Guid>>(() => response.ThrowOnFailure(result));
    Assert.Equal(result, exception.Result);
  }
}
