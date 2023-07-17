﻿using FluentValidation.Results;

namespace Logitar.Identity.Core.Users.Validators;

[Trait(Traits.Category, Categories.Unit)]
public class PersonNameValidatorTests
{
  private readonly Bogus.Faker _faker = new();
  private readonly PersonNameValidator _validator = new();

  [Theory(DisplayName = "It should use the specified property name.")]
  [InlineData("__FirstName__")]
  public void It_should_use_the_specified_property_name(string propertyName)
  {
    PersonNameValidator validator = new(propertyName);
    ValidationResult result = validator.Validate(string.Empty);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal(propertyName, failure.PropertyName);
  }

  [Theory(DisplayName = "Validation should fail when person name is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void Validation_should_fail_when_person_name_is_empty_or_only_white_space(string value)
  {
    ValidationResult result = _validator.Validate(value);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("NullOrNotEmptyValidator", failure.ErrorCode);
  }

  [Theory(DisplayName = "Validation should fail when person name is too long.")]
  [InlineData(300)]
  public void Validation_should_fail_when_person_name_is_too_long(int length)
  {
    string value = _faker.Random.String(length, minChar: 'a', maxChar: 'z');
    ValidationResult result = _validator.Validate(value);
    Assert.False(result.IsValid);
    ValidationFailure failure = result.Errors.Single();
    Assert.Equal("MaximumLengthValidator", failure.ErrorCode);
  }

  [Fact(DisplayName = "Validation should succeed when person name is valid.")]
  public void Validation_should_succeed_when_person_name_is_valid()
  {
    string value = _faker.PickRandom(new[] { _faker.Person.FirstName, _faker.Person.LastName });
    ValidationResult result = _validator.Validate(value);
    Assert.True(result.IsValid);
  }
}
