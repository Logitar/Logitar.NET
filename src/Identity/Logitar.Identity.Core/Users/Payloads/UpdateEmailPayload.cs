namespace Logitar.Identity.Core.Users.Payloads;

public record UpdateEmailPayload
{
  public string Address { get; set; } = string.Empty;
  public bool? IsVerified { get; set; }
}
