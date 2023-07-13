using FluentValidation.Results;
using Logitar.Identity.Core.ApiKeys.Events;

namespace Logitar.Identity.Core.ApiKeys.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class ApiKeyCreatedValidatorTests
{
  private readonly TitleValidator _titleValidator = new();
  private readonly ApiKeyCreatedValidator _validator;

  public ApiKeyCreatedValidatorTests()
  {
    _validator = new(_titleValidator);
  }

  [Fact(DisplayName = "Validation should fail when Secret is null.")]
  public void Validation_should_fail_when_Secret_is_null()
  {
    ApiKeyCreatedEvent e = new()
    {
      Secret = null!,
      Title = "Default"
    };
    ValidationResult result = _validator.Validate(e);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("Secret", failure.PropertyName);
    Assert.Equal("NotNullValidator", failure.ErrorCode);
  }

  [Fact(DisplayName = "Validation should fail when TenantId is not valid.")]
  public void Validation_should_fail_when_TenantId_is_not_valid()
  {
    ApiKeyCreatedEvent e = new()
    {
      TenantId = "    ",
      Title = "Default"
    };
    ValidationResult result = _validator.Validate(e);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("TenantId", failure.PropertyName);
  }

  [Fact(DisplayName = "Validation should fail when Title is not valid.")]
  public void Validation_should_fail_when_Title_is_not_valid()
  {
    ApiKeyCreatedEvent e = new()
    {
      Title = ""
    };
    ValidationResult result = _validator.Validate(e);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("Title", failure.PropertyName);
  }

  [Fact(DisplayName = "Validation should succeed when it is valid.")]
  public void Validation_should_succeed_when_it_is_valid()
  {
    ApiKeyCreatedEvent e = new()
    {
      TenantId = Guid.NewGuid().ToString(),
      Secret = new(RandomNumberGenerator.GetBytes(20)),
      Title = "Default"
    };
    ValidationResult result = _validator.Validate(e);
    Assert.True(result.IsValid);
  }
}
