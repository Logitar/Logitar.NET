using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Core.Users.Payloads;

public record UpdateEmailPayload : IEmailAddress
{
  public string Address { get; set; } = string.Empty;
  public bool? IsVerified { get; set; }
}
