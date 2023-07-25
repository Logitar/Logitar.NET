using Logitar.Identity.Domain.Settings;
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

  public static ISet<string> SupportedCountries => _countries.Keys.ToHashSet();
  public static bool IsSupported(string country) => _countries.ContainsKey(country);

  public static ICountrySettings? GetSettings(string country)
    => _countries.TryGetValue(country, out CountrySettings? settings) ? settings : null;

  public static string Format(IPostalAddress address)
  {
    List<string> lines = new();

    string[] street = address.Street.Remove("\r").Split("\n");
    foreach (string line in street)
    {
      if (!string.IsNullOrWhiteSpace(line))
      {
        lines.Add(line.Trim());
      }
    }

    string locality = string.Join(' ', new[]
    {
      address.Locality.CleanTrim(),
      address.Region?.CleanTrim(),
      address.PostalCode?.CleanTrim()
    }.Where(part => part != null));
    if (!string.IsNullOrEmpty(locality))
    {
      lines.Add(locality);
    }

    if (!string.IsNullOrWhiteSpace(address.Country))
    {
      lines.Add(address.Country.Trim());
    }

    return string.Join(Environment.NewLine, lines);
  }
}
