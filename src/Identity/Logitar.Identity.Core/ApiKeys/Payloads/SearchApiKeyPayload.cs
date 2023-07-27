using Logitar.Identity.Core.Payloads;

namespace Logitar.Identity.Core.ApiKeys.Payloads;

public record SearchApiKeyPayload : SearchPayload
{
  public TextSearch TenantId { get; set; } = new();

  public bool? IsExpired { get; set; }

  public new IEnumerable<ApiKeySortOption> Sort { get; set; } = Enumerable.Empty<ApiKeySortOption>();
}
