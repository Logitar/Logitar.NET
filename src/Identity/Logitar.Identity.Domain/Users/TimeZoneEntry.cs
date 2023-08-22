using NodaTime;

namespace Logitar.Identity.Domain.Users;

public record TimeZoneEntry
{
  private readonly DateTimeZone _dateTimeZone;

  public TimeZoneEntry(string id)
  {
    if (string.IsNullOrWhiteSpace(id))
    {
      throw new ArgumentException("The time zone identifier is required.", nameof(id));
    }

    _dateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(id.Trim())
      ?? throw new ArgumentOutOfRangeException(nameof(id), $"The time zone identifier '{id}' is not valid.");
  }

  public string Id => _dateTimeZone.Id;
}
