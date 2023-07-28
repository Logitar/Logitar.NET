namespace Logitar.Identity.Core.ApiKeys.Payloads;

public record ReplaceApiKeyPayload
{
  public string? Title { get; set; }
  public string? Description { get; set; }
  public DateTime? ExpiresOn { get; set; }

  public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
}
