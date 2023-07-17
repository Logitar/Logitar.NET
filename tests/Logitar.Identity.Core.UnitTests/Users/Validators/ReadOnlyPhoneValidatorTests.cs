using FluentValidation.Results;
using Logitar.Identity.Core.Users.Contact;

namespace Logitar.Identity.Core.Users.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class ReadOnlyPhoneValidatorTests
{
  private readonly Bogus.Faker _faker = new();

  private readonly ReadOnlyPhoneValidator _validator = new("Phone");

  [Theory(DisplayName = "Validation should fail when number is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void Validation_should_fail_when_number_is_empty_or_only_white_space(string number)
  {
    ReadOnlyPhone phone = new(number);
    ValidationResult result = _validator.Validate(phone);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "Number");
  }

  [Theory(DisplayName = "Validation should fail when number is not valid.")]
  [InlineData("test@@12..abc")]
  public void Validation_should_fail_when_number_is_not_valid(string number)
  {
    ReadOnlyPhone phone = new(number);
    ValidationResult result = _validator.Validate(phone);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("PhoneNumberValidator", failure.ErrorCode);
    Assert.Equal("Phone", failure.PropertyName);
  }

  [Theory(DisplayName = "Validation should fail when country code is too long.")]
  [InlineData("5143947377", 3)]
  public void Validation_should_fail_when_country_code_is_too_long(string number, int length)
  {
    string countryCode = _faker.Random.String(length, minChar: 'A', maxChar: 'Z');
    ReadOnlyPhone phone = new(number, countryCode);
    ValidationResult result = _validator.Validate(phone);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "CountryCode");
  }

  [Theory(DisplayName = "Validation should fail when extension is too long.")]
  [InlineData("5143947377", 13)]
  public void Validation_should_fail_when_extension_is_too_long(string number, int length)
  {
    string extension = _faker.Random.String(length, minChar: '0', maxChar: '9');
    ReadOnlyPhone phone = new(number, countryCode: "US", extension);
    ValidationResult result = _validator.Validate(phone);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "Extension");
  }

  [Theory(DisplayName = "Validation should fail when number is too long.")]
  [InlineData(25)]
  public void Validation_should_fail_when_number_is_too_long(int length)
  {
    string number = $"{_faker.Random.String(length, minChar: 'a', maxChar: 'z')}@test.com";
    ReadOnlyPhone phone = new(number);
    ValidationResult result = _validator.Validate(phone);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "Number");
  }

  [Theory(DisplayName = "Validation should succeed when it is valid.")]
  [InlineData("5143947377")]
  public void Validation_should_succeed_when_it_is_valid(string number)
  {
    ReadOnlyPhone phone = new(number);
    ValidationResult result = _validator.Validate(phone);
    Assert.True(result.IsValid);
  }
}
