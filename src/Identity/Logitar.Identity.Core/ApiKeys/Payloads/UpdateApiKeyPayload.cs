using Logitar.Identity.Core.Payloads;
using Logitar.Identity.Domain;

namespace Logitar.Identity.Core.ApiKeys.Payloads;

public record UpdateApiKeyPayload
{
  public string? Title { get; set; }
  public MayBe<string>? Description { get; set; }
  public DateTime? ExpiresOn { get; set; }

  public IEnumerable<CustomAttributeModification> CustomAttributes { get; set; } = Enumerable.Empty<CustomAttributeModification>();

  public IEnumerable<RoleModification> Roles { get; set; } = Enumerable.Empty<RoleModification>();
}
