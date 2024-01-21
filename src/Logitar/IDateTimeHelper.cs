namespace Logitar;

/// <summary>
/// Defines methods to help handling date and times.
/// </summary>
public interface IDateTimeHelper
{
  /// <summary>
  /// Returns a new <see cref="DateTime"/> instance as Coordinated Universal Time (UTC).
  /// If the date time kind is Unspecified, then it is simply specified as UTC, without any conversion.
  /// If the date time kind is Local, then it is converted to UTC.
  /// </summary>
  /// <param name="value">The date and time to convert.</param>
  /// <returns>The converted date and time.</returns>
  DateTime? ToUniversalTime(DateTime? value);
  /// <summary>
  /// Returns a new <see cref="DateTime"/> instance as Coordinated Universal Time (UTC).
  /// If the date time kind is Unspecified, then it is converted to the specified default kind, and converted to UTC if the default kind is Local.
  /// If the date time kind is Local, then it is converted to UTC.
  /// </summary>
  /// <param name="value">The date and time to convert.</param>
  /// <param name="kind">The default date time kind.</param>
  /// <returns>The converted date and time.</returns>
  DateTime? ToUniversalTime(DateTime? value, DateTimeKind kind);
  /// <summary>
  /// Returns a new <see cref="DateTime"/> instance as Coordinated Universal Time (UTC).
  /// If the date time kind is Unspecified, then it is simply specified as UTC, without any conversion.
  /// If the date time kind is Local, then it is converted to UTC.
  /// </summary>
  /// <param name="value">The date and time to convert.</param>
  /// <returns>The converted date and time.</returns>
  DateTime ToUniversalTime(DateTime value);
  /// <summary>
  /// Returns a new <see cref="DateTime"/> instance as Coordinated Universal Time (UTC).
  /// If the date time kind is Unspecified, then it is converted to the specified default kind, and converted to UTC if the default kind is Local.
  /// If the date time kind is Local, then it is converted to UTC.
  /// </summary>
  /// <param name="value">The date and time to convert.</param>
  /// <param name="kind">The default date time kind.</param>
  /// <returns>The converted date and time.</returns>
  DateTime ToUniversalTime(DateTime value, DateTimeKind kind);
}
