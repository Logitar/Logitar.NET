namespace Logitar;

[Trait("Category", "Unit")]
public class StringExtensionsTests
{
  [Theory(DisplayName = "CleanTrim: it returns null if string is null, empty, or only white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void CleanTrim_it_returns_null_if_string_is_null_empty_or_only_white_space(string s)
  {
    Assert.Null(s.CleanTrim());
  }

  [Theory(DisplayName = "CleanTrim: it returns trimmed string if it is not null, empty, or only white space.")]
  [InlineData("test")]
  [InlineData("  test  ")]
  public void CleanTrim_it_returns_trimmed_string_if_it_is_not_null_empty_or_only_white_space(string s)
  {
    Assert.Equal(s.Trim(), s.CleanTrim());
  }

  [Theory(DisplayName = "FromUriSafeBase64: uri safe base64 is correctly parsed.")]
  [InlineData("knqQH2V/ukW0IK3+UYlFaw==")]
  [InlineData("knqQH2VZukW0IK30UYlFaw8x")]
  public void FromUriSafeBase64_uri_safe_base64_is_correctly_parsed(string base64)
  {
    byte[] bytes = Convert.FromBase64String(base64);
    string s = Convert.ToBase64String(bytes).ToUriSafeBase64();
    byte[] other = Convert.FromBase64String(s.FromUriSafeBase64());
    Assert.Equal(bytes, other);
  }

  [Theory(DisplayName = "Remove: it returns the same string if no occurrence of the pattern.")]
  [InlineData("Test", "Hello World!")]
  public void Remove_it_returns_the_same_string_if_no_occurrence_of_the_pattern(string s, string pattern)
  {
    Assert.DoesNotContain(pattern, s);
    Assert.Equal(s, s.Remove(pattern));
  }

  [Theory(DisplayName = "Remove: it returns the string without pattern if any occurrence of the pattern.")]
  [InlineData("Hello World!", "o")]
  public void Remove_it_returns_the_string_without_pattern_if_any_occurrence_of_the_pattern(string s, string pattern)
  {
    Assert.Contains(pattern, s);
    Assert.Equal(s.Replace(pattern, string.Empty), s.Remove(pattern));
  }

  [Theory(DisplayName = "ToUriSafeBase64: result string is uri safe base64.")]
  [InlineData("Qe0YB5A/MUaRmUk+CXGRsg==")]
  [InlineData("Qe0YB5AcMUaRmUkpCXGRsgOy")]
  public void ToUriSafeBase64_result_string_is_uri_safe_base64(string s)
  {
    string expected = s.Replace('+', '-').Replace('/', '_').TrimEnd('=');
    string actual = s.ToUriSafeBase64();

    Assert.Equal(expected, actual);
  }

  [Theory(DisplayName = "Truncate: it returns the same string when it is not too long.")]
  [InlineData("Hello World!", 12, '.')]
  public void Truncate_it_returns_the_same_string_when_it_is_not_too_long(string s, int length, char end)
  {
    Assert.True(length >= s.Length);
    Assert.Equal(s, s.Truncate(length, end));
  }

  [Theory(DisplayName = "Truncate: it returns the truncated string when it is too long.")]
  [InlineData("Hello World!", 6, ';')]
  public void Truncate_it_returns_the_truncated_string_when_it_is_too_long(string s, int length, char end)
  {
    Assert.True(length < s.Length);

    string truncated = s.Truncate(length, end);
    Assert.EndsWith(end.ToString(), truncated);
    Assert.StartsWith(truncated[..^1], s);
  }
}
