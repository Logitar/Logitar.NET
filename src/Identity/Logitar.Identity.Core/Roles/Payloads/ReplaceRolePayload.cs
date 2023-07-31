using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.Roles.Payloads;

public record ReplaceRolePayload
{
  public string? UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public IEnumerable<CustomAttribute> CustomAttributes { get; set; } = Enumerable.Empty<CustomAttribute>();
}
