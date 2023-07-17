namespace Logitar.Identity.Core.Users.Contact;

/// <summary>
/// Static helper methods for postal addresses.
/// </summary>
public class PostalAddressHelper
{
  /// <summary>
  /// The lock to the singleton instance of the helper.
  /// </summary>
  private static readonly object _lock = new();
  /// <summary>
  /// The singleton instance of the helper.
  /// </summary>
  private static PostalAddressHelper? _instance = null;
  /// <summary>
  /// Gets the singleton instance of the helper.
  /// </summary>
  public static PostalAddressHelper Instance
  {
    get
    {
      lock (_lock)
      {
        _instance ??= new();
        return _instance;
      }
    }
  }

  /// <summary>
  /// The countries settings indexed by the country code.
  /// </summary>
  private readonly Dictionary<string, CountrySettings> _countries = new()
  {
    ["CA"] = new()
    {
      PostalCode = "[ABCEGHJ-NPRSTVXY]\\d[ABCEGHJ-NPRSTV-Z][ -]?\\d[ABCEGHJ-NPRSTV-Z]\\d$",
      Regions = ImmutableHashSet.Create(new[] { "AB", "BC", "MB", "NB", "NL", "NT", "NS", "NU", "ON", "PE", "QC", "SK", "YT" })
    }
  };

  /// <summary>
  /// Gets the list of supported countries.
  /// </summary>
  public IEnumerable<string> SupportedCountries => _countries.Keys;

  /// <summary>
  /// Returns a value indicating whether or not the specified country is supported.
  /// </summary>
  /// <param name="country">The country to validate.</param>
  /// <returns>The validation result.</returns>
  public bool IsSupported(string country) => _countries.ContainsKey(country);

  /// <summary>
  /// Gets the settings of the specified country.
  /// </summary>
  /// <param name="country">The country to get its settings.</param>
  /// <returns>The settings of the country, if found.</returns>
  public CountrySettings? GetCountry(string country)
    => _countries.TryGetValue(country, out CountrySettings? settings) ? settings : null;

  /// <summary>
  /// Sets the settings of the specified country.
  /// </summary>
  /// <param name="country">The country to set or remove its settings.</param>
  /// <param name="settings">The settings to set, or null to remove the country settings.</param>
  public void SetCountry(string country, CountrySettings? settings = null)
  {
    if (settings == null)
    {
      _countries.Remove(country);
    }
    else
    {
      _countries[country] = settings;
    }
  }
}
