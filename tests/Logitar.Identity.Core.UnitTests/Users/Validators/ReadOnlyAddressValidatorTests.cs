using FluentValidation.Results;
using Logitar.Identity.Core.Users.Contact;

namespace Logitar.Identity.Core.Users.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class ReadOnlyAddressValidatorTests
{
  private readonly Bogus.Faker _faker = new();

  private readonly ReadOnlyAddressValidator _validator = new();

  [Theory(DisplayName = "Validation should fail when country is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void Validation_should_fail_when_country_is_empty_or_only_white_space(string country)
  {
    ReadOnlyAddress address = new("Bd Roméo Vachon Nord", "Dorval", country, "QC", "H4Y 1H1");
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "Country");
  }

  [Theory(DisplayName = "Validation should fail when country is not valid.")]
  [InlineData("QC")]
  public void Validation_should_fail_when_country_is_not_valid(string country)
  {
    ReadOnlyAddress address = new("Bd Roméo Vachon Nord", "Dorval", country, "QC", "H4Y 1H1");
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("CountryValidator", failure.ErrorCode);
    Assert.Equal("Country", failure.PropertyName);
  }

  [Theory(DisplayName = "Validation should fail when country is too long.")]
  [InlineData(284)]
  public void Validation_should_fail_when_country_is_too_long(int length)
  {
    string country = _faker.Random.String(length, minChar: 'a', maxChar: 'z');
    ReadOnlyAddress address = new("Bd Roméo Vachon Nord", "Dorval", country, "QC", "H4Y 1H1");
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "Country");
  }

  [Theory(DisplayName = "Validation should fail when locality is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void Validation_should_fail_when_locality_is_empty_or_only_white_space(string locality)
  {
    ReadOnlyAddress address = new("Bd Roméo Vachon Nord", locality, "CA", "QC", "H4Y 1H1");
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "Locality");
  }

  [Theory(DisplayName = "Validation should fail when locality is too long.")]
  [InlineData(271)]
  public void Validation_should_fail_when_locality_is_too_long(int length)
  {
    string locality = _faker.Random.String(length, minChar: 'a', maxChar: 'z');
    ReadOnlyAddress address = new("Bd Roméo Vachon Nord", locality, "CA", "QC", "H4Y 1H1");
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("MaximumLengthValidator", failure.ErrorCode);
    Assert.Equal("Locality", failure.PropertyName);
  }

  [Theory(DisplayName = "Validation should fail when postal code is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void Validation_should_fail_when_postal_code_is_empty_or_only_white_space(string postalCode)
  {
    ReadOnlyAddress address = new("Bd Roméo Vachon Nord", "Dorval", "CA", "QC", postalCode);
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "PostalCode");
  }

  [Theory(DisplayName = "Validation should fail when region is not valid.")]
  [InlineData("F4Y 1H1")]
  public void Validation_should_fail_when_postal_code_is_not_valid(string postalCode)
  {
    ReadOnlyAddress address = new("Bd Roméo Vachon Nord", "Dorval", "CA", "QC", postalCode);
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("PostalCodeValidator", failure.ErrorCode);
    Assert.Equal("PostalCode", failure.PropertyName);
  }

  [Theory(DisplayName = "Validation should fail when postal code is too long.")]
  [InlineData(256)]
  public void Validation_should_fail_when_postal_code_is_too_long(int length)
  {
    string postalCode = _faker.Random.String(length, minChar: 'a', maxChar: 'z');
    ReadOnlyAddress address = new("Bd Roméo Vachon Nord", "Dorval", "CA", "QC", postalCode);
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "PostalCode");
  }

  [Theory(DisplayName = "Validation should fail when region is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void Validation_should_fail_when_region_is_empty_or_only_white_space(string region)
  {
    ReadOnlyAddress address = new("Bd Roméo Vachon Nord", "Dorval", "CA", region, "H4Y 1H1");
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "Region");
  }

  [Theory(DisplayName = "Validation should fail when region is not valid.")]
  [InlineData("ZZ")]
  public void Validation_should_fail_when_region_is_not_valid(string region)
  {
    ReadOnlyAddress address = new("Bd Roméo Vachon Nord", "Dorval", "CA", region, "H4Y 1H1");
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("RegionValidator", failure.ErrorCode);
    Assert.Equal("Region", failure.PropertyName);
  }

  [Theory(DisplayName = "Validation should fail when region is too long.")]
  [InlineData(295)]
  public void Validation_should_fail_when_region_is_too_long(int length)
  {
    string region = _faker.Random.String(length, minChar: 'a', maxChar: 'z');
    ReadOnlyAddress address = new("Bd Roméo Vachon Nord", "Dorval", "CA", region, "H4Y 1H1");
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "MaximumLengthValidator" && e.PropertyName == "Region");
  }

  [Theory(DisplayName = "Validation should fail when street address is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void Validation_should_fail_when_street_address_is_empty_or_only_white_space(string street)
  {
    ReadOnlyAddress address = new(street, "Dorval", "CA", "QC", "H4Y 1H1");
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "Street");
  }

  [Theory(DisplayName = "Validation should fail when street address is too long.")]
  [InlineData(260)]
  public void Validation_should_fail_when_street_address_is_too_long(int length)
  {
    string street = $"{_faker.Random.String(length, minChar: 'a', maxChar: 'z')}@test.com";
    ReadOnlyAddress address = new(street, "Dorval", "CA", "QC", "H4Y 1H1");
    ValidationResult result = _validator.Validate(address);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("MaximumLengthValidator", failure.ErrorCode);
    Assert.Equal("Street", failure.PropertyName);
  }

  [Theory(DisplayName = "Validation should succeed when it is valid.")]
  [InlineData("Bd Roméo Vachon Nord", "Dorval", "CA", "QC", "H4Y 1H1")]
  public void Validation_should_succeed_when_it_is_valid(string street, string locality, string country, string? region, string? postalCode)
  {
    ReadOnlyAddress email = new(street, locality, country, region, postalCode);
    ValidationResult result = _validator.Validate(email);
    Assert.True(result.IsValid);
  }
}
