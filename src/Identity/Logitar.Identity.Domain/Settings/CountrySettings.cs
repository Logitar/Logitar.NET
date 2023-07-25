namespace Logitar.Identity.Domain.Settings;

public record CountrySettings : ICountrySettings
{
  public string? PostalCode { get; set; }
  public ISet<string>? Regions { get; set; }
}
