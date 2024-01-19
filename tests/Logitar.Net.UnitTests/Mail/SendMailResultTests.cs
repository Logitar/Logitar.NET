namespace Logitar.Net.Mail;

[Trait(Traits.Category, Categories.Unit)]
public class SendMailResultTests
{
  [Fact(DisplayName = "ctor: it should construct the correct result.")]
  public void ctor_it_should_construct_the_correct_result()
  {
    SendMailResult result;

    result = new(succeeded: true);
    Assert.True(result.Succeeded);
    Assert.Empty(result.Data);

    Dictionary<string, object?> data = new()
    {
      ["Error"] = "Validation failed."
    };
    result = new(succeeded: false, data);
    Assert.False(result.Succeeded);
    Assert.Equal(data, result.Data);
  }

  [Fact(DisplayName = "Failure: it should construct the correct failed result.")]
  public void Failure_it_should_construct_the_correct_failed_result()
  {
    SendMailResult result;

    result = SendMailResult.Failure();
    Assert.False(result.Succeeded);
    Assert.Empty(result.Data);

    Dictionary<string, object?> data = new()
    {
      ["Error"] = "Validation failed."
    };
    result = SendMailResult.Failure(data);
    Assert.False(result.Succeeded);
    Assert.Equal(data, result.Data);
  }

  [Fact(DisplayName = "Success: it should construct the correct succeeded result.")]
  public void Success_it_should_construct_the_correct_succeeded_result()
  {
    SendMailResult result;

    result = SendMailResult.Success();
    Assert.True(result.Succeeded);
    Assert.Empty(result.Data);

    Dictionary<string, object?> data = new()
    {
      ["Error"] = "Validation failed."
    };
    result = SendMailResult.Success(data);
    Assert.True(result.Succeeded);
    Assert.Equal(data, result.Data);
  }
}
