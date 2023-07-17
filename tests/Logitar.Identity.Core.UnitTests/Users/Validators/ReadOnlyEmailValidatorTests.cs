using FluentValidation.Results;
using Logitar.Identity.Core.Users.Contact;

namespace Logitar.Identity.Core.Users.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class ReadOnlyEmailValidatorTests
{
  private readonly Bogus.Faker _faker = new();

  private readonly ReadOnlyEmailValidator _validator = new();

  [Theory(DisplayName = "Validation should fail when address is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void Validation_should_fail_when_address_is_empty_or_only_white_space(string address)
  {
    ReadOnlyEmail email = new(address);
    ValidationResult result = _validator.Validate(email);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "Address");
  }

  [Theory(DisplayName = "Validation should fail when address is not valid.")]
  [InlineData("test@@abc..12")]
  public void Validation_should_fail_when_address_is_not_valid(string address)
  {
    ReadOnlyEmail email = new(address);
    ValidationResult result = _validator.Validate(email);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("EmailValidator", failure.ErrorCode);
    Assert.Equal("Address", failure.PropertyName);
  }

  [Theory(DisplayName = "Validation should fail when address is too long.")]
  [InlineData(260)]
  public void Validation_should_fail_when_address_is_too_long(int length)
  {
    string address = $"{_faker.Random.String(length, minChar: 'a', maxChar: 'z')}@test.com";
    ReadOnlyEmail email = new(address);
    ValidationResult result = _validator.Validate(email);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("MaximumLengthValidator", failure.ErrorCode);
    Assert.Equal("Address", failure.PropertyName);
  }

  [Fact(DisplayName = "Validation should succeed when it is valid.")]
  public void Validation_should_succeed_when_it_is_valid()
  {
    ReadOnlyEmail email = new(_faker.Person.Email);
    ValidationResult result = _validator.Validate(email);
    Assert.True(result.IsValid);
  }
}
