using FluentValidation.Results;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users.Events;
using Logitar.Identity.Core.Validators;

namespace Logitar.Identity.Core.Users.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class UserCreatedValidatorTests
{
  private readonly UniqueNameSettings _uniqueNameSettings = new()
  {
    AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"
  };

  private readonly UniqueNameValidator _uniqueNameValidator;
  private readonly UserCreatedValidator _validator;

  public UserCreatedValidatorTests()
  {
    _uniqueNameValidator = new(_uniqueNameSettings);
    _validator = new(_uniqueNameValidator);
  }

  [Fact(DisplayName = "Validation should fail when TenantId is not valid.")]
  public void Validation_should_fail_when_TenantId_is_not_valid()
  {
    UserCreatedEvent e = new()
    {
      TenantId = "    ",
      UniqueName = "admin"
    };
    ValidationResult result = _validator.Validate(e);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("TenantId", failure.PropertyName);
  }

  [Fact(DisplayName = "Validation should fail when UniqueName is not valid.")]
  public void Validation_should_fail_when_UniqueName_is_not_valid()
  {
    UserCreatedEvent e = new()
    {
      UniqueName = "Admin88!"
    };
    ValidationResult result = _validator.Validate(e);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("UniqueName", failure.PropertyName);
  }

  [Fact(DisplayName = "Validation should succeed when it is valid.")]
  public void Validation_should_succeed_when_it_is_valid()
  {
    UserCreatedEvent e = new()
    {
      TenantId = Guid.NewGuid().ToString(),
      UniqueName = "admin"
    };
    ValidationResult result = _validator.Validate(e);
    Assert.True(result.IsValid);
  }
}
