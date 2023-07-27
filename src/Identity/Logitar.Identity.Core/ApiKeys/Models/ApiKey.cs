using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.ApiKeys.Models;

public record ApiKey : Aggregate
{
  public string? TenantId { get; set; }

  public string Title { get; set; } = string.Empty;
  public string? Description { get; set; }
  public DateTime? ExpiresOn { get; set; }

  public byte[]? Secret { get; set; }
}
