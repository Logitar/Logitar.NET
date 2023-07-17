namespace Logitar.Identity.Core.Users.Contact;

/// <summary>
/// The settings used to validate postal addresses in the country.
/// </summary>
public record CountrySettings
{
  /// <summary>
  /// Gets or sets the regular expression to validate postal codes in the country.
  /// </summary>
  public string? PostalCode { get; init; }
  /// <summary>
  /// Gets or sets the list of regions in the country.
  /// </summary>
  public ISet<string>? Regions { get; init; }
}
