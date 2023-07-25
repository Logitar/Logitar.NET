using Logitar.Identity.Core.Payloads;

namespace Logitar.Identity.Core.Users.Payloads;

public record SearchUserPayload : SearchPayload
{
  public TextSearch TenantId { get; set; } = new();

  public bool? HasPassword { get; set; }
  public bool? IsDisabled { get; set; }
  public bool? IsConfirmed { get; set; }

  public new IEnumerable<UserSortOption> Sort { get; set; } = Enumerable.Empty<UserSortOption>();
}
