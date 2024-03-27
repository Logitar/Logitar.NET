namespace Logitar.Net.Sms;

[Trait(Traits.Category, Categories.Unit)]
public class SendSmsResultTests
{
  [Fact(DisplayName = "ctor: it should construct the correct result.")]
  public void ctor_it_should_construct_the_correct_result()
  {
    SendSmsResult result;

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
    SendSmsResult result;

    result = SendSmsResult.Failure();
    Assert.False(result.Succeeded);
    Assert.Empty(result.Data);

    Dictionary<string, object?> data = new()
    {
      ["Error"] = "Validation failed."
    };
    result = SendSmsResult.Failure(data);
    Assert.False(result.Succeeded);
    Assert.Equal(data, result.Data);
  }

  [Fact(DisplayName = "Success: it should construct the correct succeeded result.")]
  public void Success_it_should_construct_the_correct_succeeded_result()
  {
    SendSmsResult result;

    result = SendSmsResult.Success();
    Assert.True(result.Succeeded);
    Assert.Empty(result.Data);

    Dictionary<string, object?> data = new()
    {
      ["Error"] = "Validation failed."
    };
    result = SendSmsResult.Success(data);
    Assert.True(result.Succeeded);
    Assert.Equal(data, result.Data);
  }
}
