using System.Collections.Immutable;

namespace Logitar.Identity.Domain.Users;

public static class PostalAddressHelper
{
  private static readonly Dictionary<string, CountrySettings> _countries = new()
  {
    ["CA"] = new CountrySettings
    {
      PostalCode = "[ABCEGHJ-NPRSTVXY]\\d[ABCEGHJ-NPRSTV-Z][ -]?\\d[ABCEGHJ-NPRSTV-Z]\\d$",
      Regions = ImmutableHashSet.Create(new[] { "AB", "BC", "MB", "NB", "NL", "NT", "NS", "NU", "ON", "PE", "QC", "SK", "YT" })
    }
  };

  public static IEnumerable<string> SupportedCountries => _countries.Keys;
  public static bool IsSupported(string country) => _countries.ContainsKey(country);

  public static CountrySettings? GetSettings(string country)
    => _countries.TryGetValue(country, out CountrySettings? settings) ? settings : null;
}
