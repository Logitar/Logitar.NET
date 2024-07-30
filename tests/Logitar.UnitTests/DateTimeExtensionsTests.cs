namespace Logitar;

[Trait(Traits.Category, Categories.Unit)]
public class DateTimeExtensionsTests
{
  [Fact(DisplayName = "AsUniversalTime: it should return a local DateTime as universal (UTC).")]
  public void AsUniversalTime_it_should_return_a_local_DateTime_as_universal_UTC()
  {
    DateTime value = DateTime.Now;
    Assert.Equal(value.ToUniversalTime(), value.AsUniversalTime());
  }

  [Fact(DisplayName = "AsUniversalTime: it should return an unspecified DateTime as universal (UTC) with the same date and time.")]
  public void AsUniversalTime_it_should_return_an_unspecified_DateTime_as_universal_UTC_with_the_same_date_and_time()
  {
    DateTime value = new(2024, 5, 21, 20, 7, 53);
    DateTime expected = new(2024, 5, 21, 20, 7, 53, DateTimeKind.Utc);
    Assert.Equal(expected, value.AsUniversalTime());
  }

  [Fact(DisplayName = "AsUniversalTime: it should return the original universal (UTC) DateTime.")]
  public void AsUniversalTime_it_should_return_the_original_universal_UTC_DateTime()
  {
    DateTime value = DateTime.UtcNow;
    Assert.Equal(value, value.AsUniversalTime());
  }

  [Fact(DisplayName = "ToISOString: it should return the correct string representation (local).")]
  public void ToISOString_it_should_return_the_correct_string_representation_local()
  {
    DateTime value = new(2024, 5, 21, 16, 7, 53, DateTimeKind.Local);
    string expected = value.ToUniversalTime().ToISOString();
    Assert.Equal(expected, value.ToISOString());
  }

  [Fact(DisplayName = "ToISOString: it should return the correct string representation (unspecified).")]
  public void ToISOString_it_should_return_the_correct_string_representation_unspecified()
  {
    DateTime value = new(2024, 5, 21, 16, 7, 53);
    string expected = DateTime.SpecifyKind(value, DateTimeKind.Utc).ToISOString();
    Assert.Equal(expected, value.ToISOString());
  }

  [Fact(DisplayName = "ToISOString: it should return the correct string representation (UTC).")]
  public void ToISOString_it_should_return_the_correct_string_representation_UTC()
  {
    DateTime value = new(2024, 5, 21, 16, 7, 53, DateTimeKind.Utc);
    Assert.Equal("2024-05-21T16:07:53.0000000Z", value.ToISOString());
  }
}
