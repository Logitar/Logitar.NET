namespace Logitar.Identity.Core;

[Trait(Traits.Category, Categories.Unit)]
public class FluentValidationExtensionsTests
{
  private const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

  [Theory(DisplayName = "BeAValidIdentifier: validation should fail when it starts with a digit or it contains an invalid character.")]
  [InlineData("321_tesT")]
  [InlineData("Test-123")]
  public void BeAValidIdentifier_validation_should_fail_when_it_starts_with_a_digit_or_it_contains_an_invalid_character(string s)
  {
    Assert.False(FluentValidationExtensions.BeAValidIdentifier(s));
  }

  [Theory(DisplayName = "BeAValidIdentifier: validation should succeed when it does not start with a digit and it does not contain any invalid character.")]
  [InlineData("Test_123")]
  public void BeAValidIdentifier_validation_should_succeed_when_it_does_not_start_with_a_digit_and_it_does_not_contain_any_invalid_character(string s)
  {
    Assert.True(FluentValidationExtensions.BeAValidIdentifier(s));
  }

  [Theory(DisplayName = "BeNullOrNotEmpty: validation should fail when it is empty or only white space.")]
  [InlineData("")]
  [InlineData("  ")]
  public void BeNullOrNotEmpty_validation_should_fail_when_it_is_empty_or_only_white_space(string s)
  {
    Assert.False(FluentValidationExtensions.BeNullOrNotEmpty(s));
  }

  [Theory(DisplayName = "BeNullOrNotEmpty: validation should succeed when it is null or not empty.")]
  [InlineData(null)]
  [InlineData(" Test9! ")]
  public void BeNullOrNotEmpty_validation_should_succeed_when_it_is_null_or_not_empty(string? s)
  {
    Assert.True(FluentValidationExtensions.BeNullOrNotEmpty(s));
  }

  [Theory(DisplayName = "Identifier: validation should fail when it contains a character that is not allowed.")]
  [InlineData("Test123!")]
  public void Identifier_validation_should_fail_when_it_contains_a_character_that_is_not_allowed(string s)
  {
    Assert.False(FluentValidationExtensions.ContainOnlyAllowedCharacters(s, AllowedCharacters));
  }

  [Theory(DisplayName = "Identifier: validation should succeed when it does not contain a character that is not allowed.")]
  [InlineData("Test@123")]
  public void Identifier_validation_should_succeed_when_it_does_not_contain_a_character_that_is_not_allowed(string s)
  {
    Assert.True(FluentValidationExtensions.ContainOnlyAllowedCharacters(s, AllowedCharacters));
  }
}
