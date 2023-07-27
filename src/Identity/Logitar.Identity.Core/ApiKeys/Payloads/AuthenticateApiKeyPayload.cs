namespace Logitar.Identity.Core.ApiKeys.Payloads;

public record AuthenticateApiKeyPayload
{
  public string Id { get; set; } = string.Empty;
  public byte[] Secret { get; set; } = Array.Empty<byte>();
}
