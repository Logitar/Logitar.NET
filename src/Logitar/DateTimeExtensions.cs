namespace Logitar;

/// <summary>
/// Provides extension methods for <see cref="DateTime"/> instances.
/// </summary>
public static class DateTimeExtensions
{
  /// <summary>
  /// Converts the specified date and time as universal (UTC).
  /// </summary>
  /// <param name="value">The date and time to convert.</param>
  /// <returns>The date and time as universal (UTC).</returns>
  /// <exception cref="ArgumentException">The date time kind is no supported.</exception>
  public static DateTime AsUniversalTime(this DateTime value) => value.Kind switch
  {
    DateTimeKind.Local => value.ToUniversalTime(),
    DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
    DateTimeKind.Utc => value,
    _ => throw new ArgumentException($"The date time kind '{value.Kind}' is not supported.", nameof(value)),
  };

  /// <summary>
  /// Converts the specified date and time as an ISO 8601 string.
  /// </summary>
  /// <param name="value">The date and time to convert.</param>
  /// <returns>The string representation.</returns>
  public static string ToISOString(this DateTime value) => value.AsUniversalTime().ToString("O", CultureInfo.InvariantCulture);
}
