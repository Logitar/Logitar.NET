using FluentValidation;
using FluentValidation.Results;

namespace Logitar.Identity.Core.Users.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class ExternalIdentifierValidatorTests
{
  private const string Key = "_Key2";
  private const string Value = "Test123!";

  private readonly ExternalIdentifierValidator _validator = new();

  [Fact(DisplayName = "It should always be the same instance.")]
  public void It_should_always_be_the_same_instance()
  {
    ExternalIdentifierValidator instance = ExternalIdentifierValidator.Instance;
    ExternalIdentifierValidator other = ExternalIdentifierValidator.Instance;
    Assert.Same(instance, other);
  }

  [Fact(DisplayName = "It should be constructed correctly.")]
  public void It_should_be_constructed_correctly()
  {
    IValidator<string> key = new ExternalIdentifierValueValidator();
    IValidator<string> value = new ExternalIdentifierKeyValidator();
    ExternalIdentifierValidator validator = new(key, value);
    Assert.Same(key, validator.Key);
    Assert.Same(value, validator.Value);
  }

  [Fact(DisplayName = "It should not throw when key and value are valid.")]
  public void It_should_not_throw_when_key_and_value_are_valid()
  {
    _validator.ValidateAndThrow(Key, Value);
  }

  [Fact(DisplayName = "It should throw ValidationException when key is not valid.")]
  public void It_should_throw_ValidationException_when_key_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _validator.ValidateAndThrow(string.Empty, Value));
    Assert.Contains(exception.Errors, e => e.PropertyName == "Key");
  }

  [Fact(DisplayName = "It should throw ValidationException when value is not valid.")]
  public void It_should_throw_ValidationException_when_value_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _validator.ValidateAndThrow(Key, string.Empty));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("Value", failure.PropertyName);
  }
}
