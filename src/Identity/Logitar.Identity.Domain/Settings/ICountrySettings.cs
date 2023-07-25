namespace Logitar.Identity.Domain.Settings;

public interface ICountrySettings
{
  string? PostalCode { get; }
  ISet<string>? Regions { get; }
}
