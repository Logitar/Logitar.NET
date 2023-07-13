using FluentValidation.Results;

namespace Logitar.Identity.Core.ApiKeys.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class ExpiresOnValidatorTests
{
  private readonly ExpiresOnValidator _validator = new();

  [Theory(DisplayName = "It should use the specified property name.")]
  [InlineData("__ExpiresOn__")]
  public void It_should_use_the_specified_property_name(string propertyName)
  {
    ExpiresOnValidator validator = new(propertyName);
    ValidationResult result = validator.Validate(DateTime.Now);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal(propertyName, failure.PropertyName);
  }

  [Fact(DisplayName = "Validation should fail when date is not in the future.")]
  public void Validation_should_fail_when_date_is_not_in_the_future()
  {
    ValidationResult result = _validator.Validate(DateTime.Now.AddHours(-10));
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("FutureValidator", failure.ErrorCode);
  }

  [Fact(DisplayName = "Validation should succeed when date is in the future.")]
  public void Validation_should_succeed_when_date_is_in_the_future()
  {
    ValidationResult result = _validator.Validate(DateTime.Now.AddHours(10));
    Assert.True(result.IsValid);
  }
}
