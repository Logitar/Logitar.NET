using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Users.Models;

namespace Logitar.Identity.Core.Sessions.Models;

public record Session : Aggregate
{
  public bool IsPersistent { get; set; }

  public bool IsActive { get; set; }
  public Actor? SignedOutBy { get; set; }
  public DateTime? SignedOutOn { get; set; }

  public string? RefreshToken { get; set; }

  public User User { get; set; } = new();

  public IEnumerable<CustomAttribute> CustomAttributes { get; set; } = Enumerable.Empty<CustomAttribute>();
}
