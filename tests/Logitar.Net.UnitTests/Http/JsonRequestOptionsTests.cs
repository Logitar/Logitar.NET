using Bogus;

namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class JsonRequestOptionsTests
{
  private readonly Faker _faker = new();
  private readonly Person _person;

  public JsonRequestOptionsTests()
  {
    _person = new(_faker.Person.FirstName, _faker.Person.LastName, _faker.Person.DateOfBirth);
  }

  [Fact(DisplayName = "ctor: it should construct the correct options.")]
  public void ctor_it_should_construct_the_correct_options()
  {
    JsonRequestOptions options;

    options = new();
    Assert.Null(options.Content);

    options = new(_person);
    Assert.NotNull(options.Content);
    Assert.True(options.Content is JsonContent jsonContent && jsonContent.Value != null && jsonContent.Value.Equals(_person));
  }

  [Fact(DisplayName = "GetContactValue: it should return the correct value when the result has contents.")]
  public void GetContactValue_it_should_return_the_correct_value_when_the_result_has_contents()
  {
    JsonRequestOptions options = new(_person);
    Person? person = options.GetContentValue<Person>();
    Assert.NotNull(person);
    Assert.Equal(_person, person);
  }

  [Fact(DisplayName = "GetContentValue: it should return the default value when the result has no content.")]
  public void GetContentValue_it_should_return_the_default_value_when_the_result_has_no_content()
  {
    JsonRequestOptions options = new();
    Assert.Null(options.GetContentValue<Person>());
  }

  [Fact(DisplayName = "SetContentValue: it should remove the content when the value is null.")]
  public void SetContentValue_it_should_remove_the_content_when_the_value_is_null()
  {
    JsonRequestOptions options = new(_person);
    Assert.NotNull(options.Content);

    options.SetContentValue(value: null);
    Assert.Null(options.Content);
  }

  [Fact(DisplayName = "SetContentValue: it should set the correct content value.")]
  public void SetContentValue_it_should_set_the_correct_content_value()
  {
    JsonRequestOptions options = new();
    Assert.Null(options.Content);

    options.SetContentValue(_person);
    Assert.NotNull(options.Content);
    Assert.True(options.Content is JsonContent jsonContent && jsonContent.Value != null && jsonContent.Value.Equals(_person));
  }
}
