using NodaTime;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents an entry in the tz database. See <see href="https://en.wikipedia.org/wiki/Tz_database"/> for more detail.
/// <br />The list of entries can be found here: <see href="https://en.wikipedia.org/wiki/List_of_tz_database_time_zones"/>.
/// </summary>
public record TimeZone
{
  /// <summary>
  /// The NodaTime time zone represented by this time zone.
  /// </summary>
  private readonly DateTimeZone _dateTimeZone;

  /// <summary>
  /// Initializes a new instance of the <see cref="TimeZone"/> class.
  /// </summary>
  /// <param name="id">The time zone identifier.</param>
  /// <exception cref="ArgumentException">The time zone identifier was empty or only white space.</exception>
  /// <exception cref="ArgumentOutOfRangeException">The time zone could not be found.</exception>
  public TimeZone(string id)
  {
    if (string.IsNullOrWhiteSpace(id))
    {
      throw new ArgumentException("The time zone identifier is required.", nameof(id));
    }

    _dateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(id.Trim())
      ?? throw new ArgumentOutOfRangeException(nameof(id));
  }

  /// <summary>
  /// Gets the identifier of the time zone.
  /// </summary>
  public string Id => _dateTimeZone.Id;
}
