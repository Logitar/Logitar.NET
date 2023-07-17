using FluentValidation.Results;

namespace Logitar.Identity.Core.Users.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class BirthdateValidatorTests
{
  private readonly BirthdateValidator _validator = new();

  [Theory(DisplayName = "It should use the specified property name.")]
  [InlineData("__Birthdate__")]
  public void It_should_use_the_specified_property_name(string propertyName)
  {
    BirthdateValidator validator = new(propertyName);
    ValidationResult result = validator.Validate(DateTime.Now.AddYears(20));
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal(propertyName, failure.PropertyName);
  }

  [Fact(DisplayName = "Validation should fail when date is not in the past.")]
  public void Validation_should_fail_when_date_is_not_in_the_past()
  {
    ValidationResult result = _validator.Validate(DateTime.Now.AddHours(10));
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("PastValidator", failure.ErrorCode);
  }

  [Fact(DisplayName = "Validation should succeed when date is in the past.")]
  public void Validation_should_succeed_when_date_is_in_the_past()
  {
    ValidationResult result = _validator.Validate(DateTime.Now.AddHours(-10));
    Assert.True(result.IsValid);
  }
}

