using Logitar.Identity.Core.Payloads;

namespace Logitar.Identity.Core.Sessions.Payloads;

public record SearchSessionPayload : SearchPayload
{
  public TextSearch TenantId { get; set; } = new();
  public TextSearch UserId { get; set; } = new();

  public bool? IsPersistent { get; set; }
  public bool? IsActive { get; set; }

  public new IEnumerable<SessionSortOption> Sort { get; set; } = Enumerable.Empty<SessionSortOption>();
}
