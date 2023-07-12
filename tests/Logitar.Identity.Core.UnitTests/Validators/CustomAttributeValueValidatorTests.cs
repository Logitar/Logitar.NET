using FluentValidation.Results;

namespace Logitar.Identity.Core.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class CustomAttributeValueValidatorTests
{
  private readonly Bogus.Faker _faker = new();

  private readonly CustomAttributeValueValidator _validator = new();

  [Theory(DisplayName = "It should use the specified property name.")]
  [InlineData("__CustomAttributeValue__")]
  public void It_should_use_the_specified_property_name(string propertyName)
  {
    CustomAttributeValueValidator validator = new(propertyName);
    ValidationResult result = validator.Validate(string.Empty);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal(propertyName, failure.PropertyName);
  }

  [Theory(DisplayName = "Validation should fail when value is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void Validation_should_fail_when_value_is_empty_or_only_white_space(string value)
  {
    ValidationResult result = _validator.Validate(value);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("NotEmptyValidator", failure.ErrorCode);
  }

  [Theory(DisplayName = "Validation should success when value is valid.")]
  [InlineData("_Test123")]
  public void Validation_should_success_when_value_is_valid(string value)
  {
    ValidationResult result = _validator.Validate(value);
    Assert.True(result.IsValid);
  }
}
