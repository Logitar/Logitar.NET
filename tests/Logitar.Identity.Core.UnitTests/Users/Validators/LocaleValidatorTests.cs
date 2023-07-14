using FluentValidation.Results;
using System.Globalization;

namespace Logitar.Identity.Core.Users.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class LocaleValidatorTests
{
  private readonly LocaleValidator _validator = new();

  [Theory(DisplayName = "It should use the specified property name.")]
  [InlineData("__Locale__")]
  public void It_should_use_the_specified_property_name(string propertyName)
  {
    LocaleValidator validator = new(propertyName);
    ValidationResult result = validator.Validate(CultureInfo.InvariantCulture);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal(propertyName, failure.PropertyName);
  }

  [Fact(DisplayName = "Validation should fail when locale is not valid.")]
  public void Validation_should_fail_when_locale_is_not_valid()
  {
    ValidationResult result = _validator.Validate(new CultureInfo("fr-US"));
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("LocaleValidator", failure.ErrorCode);
  }

  [Fact(DisplayName = "Validation should succeed when locale is valid.")]
  public void Validation_should_succeed_when_date_is_in_the_past()
  {
    ValidationResult result = _validator.Validate(new CultureInfo("fr-CA"));
    Assert.True(result.IsValid);
  }
}

