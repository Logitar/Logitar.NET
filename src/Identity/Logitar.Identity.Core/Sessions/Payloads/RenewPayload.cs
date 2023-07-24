namespace Logitar.Identity.Core.Sessions.Payloads;

public record RenewPayload
{
  public string RefreshToken { get; set; } = string.Empty;
}
