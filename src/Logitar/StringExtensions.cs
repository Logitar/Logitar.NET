namespace Logitar;

/// <summary>
/// Provides extension methods for <see cref="string"/> instances.
/// </summary>
public static class StringExtensions
{
  /// <summary>
  /// Returns the specified string as trimmed, or null if it is null, empty or only white space.
  /// </summary>
  /// <param name="s">The string to clean or trim.</param>
  /// <returns>The trimmed string or null.</returns>
  public static string? CleanTrim(this string s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

  /// <summary>
  /// Returns the original base64 string from its Uri-safe version.
  /// </summary>
  /// <param name="s">The Uri-safe base64 string.</param>
  /// <returns>The original base64 string.</returns>
  public static string FromUriSafeBase64(this string s)
  {
    if (s.Length % 4 > 0)
    {
      s = s.PadRight((s.Length / 4 + 1) * 4, '=');
    }

    return s.Replace('_', '/').Replace('-', '+');
  }
  /// <summary>
  /// Returns an Uri-safe base64 version of the specified base64 string.
  /// </summary>
  /// <param name="s">The base64 string.</param>
  /// <returns>The Uri-safe base6 version of the string.</returns>
  public static string ToUriSafeBase64(this string s) => s.Replace('+', '-').Replace('/', '_').TrimEnd('=');

  /// <summary>
  /// Humanizes the specified string by inserting spaces between words. Words are separated by case-change, such as in camelCase and PascalCase.
  /// </summary>
  /// <param name="s">The camelCase or PascalCase string.</param>
  /// <returns>The humanized string.</returns>
  public static string Humanize(this string s)
  {
    List<string> words = new(capacity: s.Length);

    StringBuilder word = new();
    for (int i = 0; i < s.Length; i++)
    {
      char? previous = (i > 0) ? s[i - 1] : null;
      char current = s[i];
      char? next = (i < s.Length - 1) ? s[i + 1] : null;

      if (char.IsUpper(current) && ((previous.HasValue && char.IsLower(previous.Value)) || (next.HasValue && char.IsLower(next.Value))))
      {
        if (word.Length > 0)
        {
          words.Add(word.ToString());
          word.Clear();
        }
      }

      word.Append(current);
    }
    if (word.Length > 0)
    {
      words.Add(word.ToString());
    }

    return string.Join(' ', words);
  }

  /// <summary>
  /// Returns a new string where every character of the specified string has been replaced by the specified mask character (defaults to '*').
  /// </summary>
  /// <param name="s">The original string.</param>
  /// <param name="mask">The mask character (defaults to '*').</param>
  /// <returns>The masked string.</returns>
  public static string Mask(this string s, char mask = '*')
  {
    char[] masked = new char[s.Length];
    Array.Fill(masked, mask);
    return new string(masked);
  }

  /// <summary>
  /// Removes all occurrences of the specified pattern from the specified string.
  /// </summary>
  /// <param name="s">The string to remove the pattern.</param>
  /// <param name="pattern">The pattern to remove.</param>
  /// <returns>The string without any occurrence of the pattern.</returns>
  public static string Remove(this string s, string pattern) => s.Replace(pattern, string.Empty);

  /// <summary>
  /// Truncates the specified string to the specified length, using the specified end character.
  /// </summary>
  /// <param name="s">The string to truncate.</param>
  /// <param name="length">The maximum length of the truncated string.</param>
  /// <param name="end">The end character of the truncated string.</param>
  /// <returns>The truncated string.</returns>
  public static string Truncate(this string s, int length, char end = '…')
  {
    if (s.Length > length)
    {
      return string.Concat(s[..(length - 1)], end);
    }

    return s;
  }
}
