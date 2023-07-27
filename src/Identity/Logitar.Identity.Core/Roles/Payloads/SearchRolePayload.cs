using Logitar.Identity.Core.Payloads;

namespace Logitar.Identity.Core.Roles.Payloads;

public record SearchRolePayload : SearchPayload
{
  public TextSearch TenantId { get; set; } = new();

  public new IEnumerable<RoleSortOption> Sort { get; set; } = Enumerable.Empty<RoleSortOption>();
}
