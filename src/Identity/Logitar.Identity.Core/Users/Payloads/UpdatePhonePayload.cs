using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Core.Users.Payloads;

public record UpdatePhonePayload : IPhoneNumber
{
  public string? CountryCode { get; set; }
  public string Number { get; set; } = string.Empty;
  public string? Extension { get; set; }
  public bool? IsVerified { get; set; }
}
