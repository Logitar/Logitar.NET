using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.Roles.Models;

public record Role : Aggregate
{
  public string? TenantId { get; set; }

  public string UniqueName { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public IEnumerable<CustomAttribute> CustomAttributes { get; set; } = Enumerable.Empty<CustomAttribute>();
}
