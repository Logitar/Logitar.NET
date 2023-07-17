using FluentValidation.Results;

namespace Logitar.Identity.Core.Users.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class ExternalIdentifierKeyValidatorTests
{
  private readonly Bogus.Faker _faker = new();

  private readonly ExternalIdentifierKeyValidator _validator = new();

  [Theory(DisplayName = "It should use the specified property name.")]
  [InlineData("__ExternalIdentifierKey__")]
  public void It_should_use_the_specified_property_name(string propertyName)
  {
    ExternalIdentifierKeyValidator validator = new(propertyName);
    ValidationResult result = validator.Validate(string.Empty);
    Assert.False(result.IsValid);
    Assert.All(result.Errors, e => Assert.Equal(propertyName, e.PropertyName));
  }

  [Theory(DisplayName = "Validation should fail when key is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void Validation_should_fail_when_key_is_empty_or_only_white_space(string value)
  {
    ValidationResult result = _validator.Validate(value);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "NotEmptyValidator");
  }

  [Theory(DisplayName = "Validation should fail when key is not a valid identifier.")]
  [InlineData("Admin_123!")]
  [InlineData("123_Admin")]
  public void Validation_should_fail_when_key_is_not_a_valid_identifier(string value)
  {
    ValidationResult result = _validator.Validate(value);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("IdentifierValidator", failure.ErrorCode);
  }

  [Theory(DisplayName = "Validation should fail when key is too long.")]
  [InlineData(300)]
  public void Validation_should_fail_when_key_is_too_long(int length)
  {
    string value = _faker.Random.String(length, minChar: 'a', maxChar: 'z');
    ValidationResult result = _validator.Validate(value);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("MaximumLengthValidator", failure.ErrorCode);
  }

  [Theory(DisplayName = "Validation should succeed when key is valid.")]
  [InlineData("_Test123")]
  public void Validation_should_succeed_when_key_is_valid(string value)
  {
    ValidationResult result = _validator.Validate(value);
    Assert.True(result.IsValid);
  }
}
