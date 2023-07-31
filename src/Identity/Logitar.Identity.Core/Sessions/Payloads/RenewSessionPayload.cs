using Logitar.Identity.Core.Payloads;

namespace Logitar.Identity.Core.Sessions.Payloads;

public record RenewSessionPayload
{
  public string RefreshToken { get; set; } = string.Empty;

  public IEnumerable<CustomAttributeModification> CustomAttributes { get; set; } = Enumerable.Empty<CustomAttributeModification>();
}
