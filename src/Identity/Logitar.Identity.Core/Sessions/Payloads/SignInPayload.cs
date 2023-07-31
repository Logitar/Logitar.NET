using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.Sessions.Payloads;

public record SignInPayload
{
  public string? TenantId { get; set; }

  public string UniqueName { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;

  public bool IsPersistent { get; set; }

  public IEnumerable<CustomAttribute> CustomAttributes { get; set; } = Enumerable.Empty<CustomAttribute>();
}
