namespace Logitar;

/// <summary>
/// Implements methods to help handling date and times.
/// </summary>
public class DateTimeHelper : IDateTimeHelper
{
  private static IDateTimeHelper? _instance = null;
  /// <summary>
  /// Gets the singleton instance of the date time helper.
  /// </summary>
  public static IDateTimeHelper Instance
  {
    get
    {
      _instance ??= new DateTimeHelper();
      return _instance;
    }
  }

  /// <summary>
  /// Returns a new <see cref="DateTime"/> instance as Coordinated Universal Time (UTC).
  /// If the date time kind is Unspecified, then it is simply specified as UTC, without any conversion.
  /// If the date time kind is Local, then it is converted to UTC.
  /// </summary>
  /// <param name="value">The date and time to convert.</param>
  /// <returns>The converted date and time.</returns>
  public virtual DateTime? ToUniversalTime(DateTime? value)
  {
    return ToUniversalTime(value, DateTimeKind.Utc);
  }
  /// <summary>
  /// Returns a new <see cref="DateTime"/> instance as Coordinated Universal Time (UTC).
  /// If the date time kind is Unspecified, then it is converted to the specified default kind, and converted to UTC if the default kind is Local.
  /// If the date time kind is Local, then it is converted to UTC.
  /// </summary>
  /// <param name="value">The date and time to convert.</param>
  /// <param name="kind">The default date time kind.</param>
  /// <returns>The converted date and time.</returns>
  public virtual DateTime? ToUniversalTime(DateTime? value, DateTimeKind kind)
  {
    return value.HasValue ? ToUniversalTime(value.Value) : null;
  }
  /// <summary>
  /// Returns a new <see cref="DateTime"/> instance as Coordinated Universal Time (UTC).
  /// If the date time kind is Unspecified, then it is simply specified as UTC, without any conversion.
  /// If the date time kind is Local, then it is converted to UTC.
  /// </summary>
  /// <param name="value">The date and time to convert.</param>
  /// <returns>The converted date and time.</returns>
  public virtual DateTime ToUniversalTime(DateTime value)
  {
    return ToUniversalTime(value, DateTimeKind.Utc);
  }
  /// <summary>
  /// Returns a new <see cref="DateTime"/> instance as Coordinated Universal Time (UTC).
  /// If the date time kind is Unspecified, then it is converted to the specified default kind, and converted to UTC if the default kind is Local.
  /// If the date time kind is Local, then it is converted to UTC.
  /// </summary>
  /// <param name="value">The date and time to convert.</param>
  /// <param name="kind">The default date time kind.</param>
  /// <returns>The converted date and time.</returns>
  public virtual DateTime ToUniversalTime(DateTime value, DateTimeKind kind)
  {
    if (kind == DateTimeKind.Unspecified)
    {
      throw new ArgumentOutOfRangeException(nameof(kind), $"The default date time kind cannot be '{DateTimeKind.Unspecified}'.");
    }

    if (value.Kind == DateTimeKind.Utc)
    {
      return value;
    }
    else if (value.Kind == DateTimeKind.Unspecified)
    {
      value = DateTime.SpecifyKind(value, kind);
    }

    return value.ToUniversalTime();
  }
}

// TODO(fpion): unit tests
