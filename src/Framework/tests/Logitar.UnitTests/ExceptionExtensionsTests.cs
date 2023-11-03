namespace Logitar;

[Trait(Traits.Category, Categories.Unit)]
public class ExceptionExtensionsTests
{
  [Fact(DisplayName = "GetErrorCode: it should return the correct error code.")]
  public void GetErrorCode_it_should_return_the_correct_error_code()
  {
    InvalidOperationException exception = new();
    Assert.Equal("InvalidOperation", exception.GetErrorCode());
  }
}
