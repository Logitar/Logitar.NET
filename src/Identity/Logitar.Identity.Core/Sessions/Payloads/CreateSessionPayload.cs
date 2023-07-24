namespace Logitar.Identity.Core.Sessions.Payloads;

public record CreateSessionPayload
{
  public string UserId { get; set; } = string.Empty;

  public bool IsPersistent { get; set; }
}
