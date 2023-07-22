using Logitar.Identity.Core.Models;

namespace Logitar.Identity.Core.Users.Models;

public abstract record Contact
{
  public bool IsVerified { get; set; }
  public Actor? VerifiedBy { get; set; }
  public DateTime? VerifiedOn { get; set; }
}
