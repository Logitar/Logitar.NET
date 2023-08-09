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

  public static string Format(IPostalAddress address)
  {
    List<string> lines = new();

    string[] streetLines = address.Street.Remove("\r").Split('\n');
    foreach (string streetLine in streetLines)
    {
      if (!string.IsNullOrWhiteSpace(streetLine))
      {
        lines.Add(streetLine.Trim());
      }
    }

    string?[] values = new[]
    {
      address.Locality.CleanTrim(),
      address.Region?.CleanTrim(),
      address.PostalCode?.CleanTrim()
    };
    string localityRegionPostalCode = string.Join(' ', values.Where(value => !string.IsNullOrEmpty(value)));
    if (!string.IsNullOrEmpty(localityRegionPostalCode))
    {
      lines.Add(localityRegionPostalCode);
    }

    if (!string.IsNullOrWhiteSpace(address.Country))
    {
      lines.Add(address.Country.Trim());
    }

    return string.Join(Environment.NewLine, lines);
  }
}
