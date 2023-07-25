using NodaTime;

namespace Logitar.Identity.Domain.Users;

public record TimeZoneEntry
{
  private readonly DateTimeZone _timeZone;

  public TimeZoneEntry(string id)
  {
    if (string.IsNullOrWhiteSpace(id))
    {
      throw new ArgumentException("The time zone identifier is required.", nameof(id));
    }

    _timeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(id.Trim())
      ?? throw new ArgumentOutOfRangeException(nameof(id), $"The time zone '{id}' could not be found.");
  }

  public string Id => _timeZone.Id;
}
