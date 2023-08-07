namespace Logitar.Identity.Domain.Users;

public record CountrySettings
{
  public string? PostalCode { get; init; }
  public ISet<string>? Regions { get; init; }
}
