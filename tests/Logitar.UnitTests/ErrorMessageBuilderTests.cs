namespace Logitar;

[Trait(Traits.Category, Categories.Unit)]
public class ErrorMessageBuilderTests
{
  [Theory(DisplayName = "AddData: it can add data with a default value.")]
  [InlineData("test", "<null>")]
  public void AddData_it_can_add_data_with_a_default_value(string key, string defaultValue)
  {
    string message = new ErrorMessageBuilder().AddData(key, value: null, defaultValue).Build();
    Assert.Equal($"{key}: {defaultValue}", message);
  }

  [Theory(DisplayName = "AddData: it can add data with a null value.")]
  [InlineData("test")]
  public void AddData_it_can_add_data_with_a_null_value(string key)
  {
    string message = new ErrorMessageBuilder().AddData(key, value: null).Build();
    Assert.Equal($"{key}: ", message);
  }

  [Fact(DisplayName = "Build: it can be empty.")]
  public void Build_it_can_be_empty()
  {
    string message = new ErrorMessageBuilder().Build();
    Assert.Empty(message);
  }

  [Fact(DisplayName = "Build: it should not start or end with a new line.")]
  public void Build_it_should_not_start_or_end_with_a_new_line()
  {
    Guid id = Guid.NewGuid();
    string message = new ErrorMessageBuilder().AddData("Id", id).Build();
    Assert.Equal($"Id: {id}", message);
  }

  [Theory(DisplayName = "Build: it should start with the initial message.")]
  [InlineData("An error has occurred.")]
  public void Build_it_should_start_with_the_initial_message(string message)
  {
    string built = new ErrorMessageBuilder(message).Build();
    Assert.StartsWith(message, built);
  }
}
