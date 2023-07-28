using System.Collections.Immutable;

namespace Logitar.Identity.Domain.Settings;

public interface ICountrySettings
{
  string? PostalCode { get; }
  IImmutableSet<string>? Regions { get; }
}
