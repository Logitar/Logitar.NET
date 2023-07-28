using System.Collections.Immutable;

namespace Logitar.Identity.Domain.Settings;

public record CountrySettings : ICountrySettings
{
  public string? PostalCode { get; set; }
  public IImmutableSet<string>? Regions { get; set; }
}
