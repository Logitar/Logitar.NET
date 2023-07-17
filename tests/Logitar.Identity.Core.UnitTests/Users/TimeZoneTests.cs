namespace Logitar.Identity.Core.Users;

[Trait(Traits.Category, Categories.Unit)]
public class TimeZoneTests
{
  [Theory(DisplayName = "It should construct the correct time zone.")]
  [InlineData("  America/Montreal  ")]
  public void It_should_construct_the_correct_time_zone(string id)
  {
    TimeZone timeZone = new(id);
    Assert.Equal(id.Trim(), timeZone.Id);
  }

  [Theory(DisplayName = "It should throw ArgumentException when identifier is empty or only white space.")]
  [InlineData("")]
  [InlineData("   ")]
  public void It_should_throw_ArgumentException_when_identifier_is_empty_or_only_white_space(string id)
  {
    var exception = Assert.Throws<ArgumentException>(() => new TimeZone(id));
    Assert.Equal("id", exception.ParamName);
  }

  [Theory(DisplayName = "It should throw ArgumentOutOfRangeException when time zone is not found.")]
  [InlineData("America/Quebec")]
  public void It_should_throw_ArgumentOutOfRangeException_when_time_zone_is_not_found(string id)
  {
    var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new TimeZone(id));
    Assert.Equal("id", exception.ParamName);
  }
}
