namespace Logitar.Identity.Core.Sessions.Payloads;

public record RenewSessionPayload
{
  public string RefreshToken { get; set; } = string.Empty;
}
