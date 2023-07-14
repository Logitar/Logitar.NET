using FluentValidation.Results;
using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Core.Users.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class PasswordValidatorTests
{
  private readonly PasswordSettings _passwordSettings = new()
  {
    RequiredLength = 8,
    RequiredUniqueChars = 6,
    RequireNonAlphanumeric = true,
    RequireLowercase = true,
    RequireUppercase = true,
    RequireDigit = true
  };

  private readonly PasswordValidator _validator;

  public PasswordValidatorTests()
  {
    _validator = new(_passwordSettings);
  }

  [Theory(DisplayName = "It should use the specified property name.")]
  [InlineData("__UniqueName__")]
  public void It_should_use_the_specified_property_name(string propertyName)
  {
    PasswordValidator validator = new(_passwordSettings, propertyName);
    ValidationResult result = validator.Validate(string.Empty);
    Assert.False(result.IsValid);
    Assert.All(result.Errors, e => Assert.Equal(propertyName, e.PropertyName));
  }

  [Fact(DisplayName = "Validation should fail password does not contain a digit.")]
  public void Validation_should_fail_password_does_not_contain_a_digit()
  {
    ValidationResult result = _validator.Validate("Test!@/$");
    Assert.False(result.IsValid);
    Assert.Equal("PasswordRequiresDigit", result.Errors.Single().ErrorCode);
  }

  [Fact(DisplayName = "Validation should fail password does not contain a lowercase letter.")]
  public void Validation_should_fail_password_does_not_contain_a_lowercase_letter()
  {
    ValidationResult result = _validator.Validate("TEST123!");
    Assert.False(result.IsValid);
    Assert.Equal("PasswordRequiresLower", result.Errors.Single().ErrorCode);
  }

  [Fact(DisplayName = "Validation should fail password does not contain a non-alphanumeric character.")]
  public void Validation_should_fail_password_does_not_contain_a_non_alphanumeric_character()
  {
    ValidationResult result = _validator.Validate("Test1234");
    Assert.False(result.IsValid);
    Assert.Equal("PasswordRequiresNonAlphanumeric", result.Errors.Single().ErrorCode);
  }

  [Fact(DisplayName = "Validation should fail password does not contain an uppercase letter.")]
  public void Validation_should_fail_password_does_not_contain_an_uppercase_letter()
  {
    ValidationResult result = _validator.Validate("test123!");
    Assert.False(result.IsValid);
    Assert.Equal("PasswordRequiresUpper", result.Errors.Single().ErrorCode);
  }

  [Fact(DisplayName = "Validation should fail password does not contain enough unique characters.")]
  public void Validation_should_fail_password_does_not_contain_enough_unique_characters()
  {
    ValidationResult result = _validator.Validate("AAaa!!11");
    Assert.False(result.IsValid);
    Assert.Equal("PasswordRequiresUniqueChars", result.Errors.Single().ErrorCode);
  }

  [Theory(DisplayName = "Validation should fail when password is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void Validation_should_fail_when_password_is_empty_or_only_white_space(string value)
  {
    ValidationResult result = _validator.Validate(value);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "NotEmptyValidator");
  }

  [Fact(DisplayName = "Validation should fail when password is too short.")]
  public void Validation_should_fail_when_password_is_too_short()
  {
    ValidationResult result = _validator.Validate("Test12!");
    Assert.False(result.IsValid);
    Assert.Equal("PasswordTooShort", result.Errors.Single().ErrorCode);
  }

  [Fact(DisplayName = "Validation should succeed when password is valid.")]
  public void Validation_should_succeed_when_password_is_valid()
  {
    ValidationResult result = _validator.Validate("Test123!");
    Assert.True(result.IsValid);
  }
}
