namespace Logitar.Identity.Core.Users;

[Trait(Traits.Category, Categories.Unit)]
public class GenderTests
{
  private readonly Bogus.Faker _faker = new();

  [Theory(DisplayName = "It should throw ArgumentException when value is empty or only white space.")]
  [InlineData("")]
  [InlineData("     ")]
  public void It_should_throw_ArgumentException_when_value_is_empty_or_only_white_space(string value)
  {
    var exception = Assert.Throws<ArgumentException>(() => new Gender(value));
    Assert.Equal("value", exception.ParamName);
  }

  [Theory(DisplayName = "It should throw ArgumentOutOfRangeException when value is too long.")]
  [InlineData(297)]
  public void It_should_throw_ArgumentOutOfRangeException_when_value_is_too_long(int length)
  {
    string value = _faker.Random.String(length, minChar: 'a', maxChar: 'z');
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Gender(value));
    Assert.Equal("value", exception.ParamName);
  }

  [Fact(DisplayName = "It should use defined value when value is known.")]
  public void It_should_use_defined_value_when_value_is_known()
  {
    Gender gender = new(_faker.Person.Gender.ToString());
    Assert.Equal(_faker.Person.Gender.ToString().ToLower(), gender.Value);
    Assert.True(Gender.KnownGenders.Contains(gender.Value));
  }

  [Theory(DisplayName = "It should use specified value when value is not known.")]
  [InlineData("  Other  ")]
  public void It_should_use_specified_value_when_value_is_not_known(string value)
  {
    Gender gender = new(value);
    Assert.Equal(value.Trim(), gender.Value);
    Assert.False(Gender.KnownGenders.Contains(gender.Value));
  }
}
