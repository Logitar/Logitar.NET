using FluentValidation.Results;
using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Core.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class UniqueNameValidatorTests
{
  private readonly Bogus.Faker _faker = new();

  private readonly UniqueNameSettings _uniqueNameSettings = new()
  {
    AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"
  };
  private readonly UniqueNameValidator _validator;

  public UniqueNameValidatorTests()
  {
    _validator = new(_uniqueNameSettings);
  }

  [Theory(DisplayName = "It should use the specified property name.")]
  [InlineData("__UniqueName__")]
  public void It_should_use_the_specified_property_name(string propertyName)
  {
    UniqueNameValidator validator = new(_uniqueNameSettings, propertyName);
    ValidationResult result = validator.Validate(string.Empty);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal(propertyName, failure.PropertyName);
  }

  [Theory(DisplayName = "Validation should fail when unique name contains a character that is not allowed.")]
  [InlineData("admin!")]
  public void Validation_should_fail_when_unique_name_contains_a_character_that_is_not_allowed(string value)
  {
    ValidationResult result = _validator.Validate(value);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("AllowedCharactersValidator", failure.ErrorCode);
  }

  [Theory(DisplayName = "Validation should fail when unique name is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void Validation_should_fail_when_unique_name_is_empty_or_only_white_space(string value)
  {
    ValidationResult result = _validator.Validate(value);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "NotEmptyValidator");
  }

  [Theory(DisplayName = "Validation should fail when unique name is too long.")]
  [InlineData(300)]
  public void Validation_should_fail_when_unique_name_is_too_long(int length)
  {
    string value = _faker.Random.String(length, minChar: 'a', maxChar: 'z');
    ValidationResult result = _validator.Validate(value);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("MaximumLengthValidator", failure.ErrorCode);
  }

  [Theory(DisplayName = "Validation should succeed when unique name is valid.")]
  [InlineData("admin_123")]
  public void Validation_should_succeed_when_unique_name_is_valid(string value)
  {
    ValidationResult result = _validator.Validate(value);
    Assert.True(result.IsValid);
  }
}
