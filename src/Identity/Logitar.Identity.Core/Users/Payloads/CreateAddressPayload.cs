using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Core.Users.Payloads;

public record CreateAddressPayload : IPostalAddress
{
  public string Street { get; set; } = string.Empty;
  public string Locality { get; set; } = string.Empty;
  public string Country { get; set; } = string.Empty;
  public string? Region { get; set; }
  public string? PostalCode { get; set; }
  public bool IsVerified { get; set; }
}
