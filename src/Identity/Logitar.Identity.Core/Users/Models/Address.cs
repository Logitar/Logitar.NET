using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Core.Users.Models;

public record Address : Contact, IPostalAddress
{
  public string Street { get; set; } = string.Empty;
  public string Locality { get; set; } = string.Empty;
  public string Country { get; set; } = string.Empty;
  public string? Region { get; set; }
  public string? PostalCode { get; set; }
  public string Formatted { get; set; } = string.Empty;
}
