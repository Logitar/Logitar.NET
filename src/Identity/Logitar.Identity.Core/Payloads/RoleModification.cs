using Logitar.Identity.Domain;

namespace Logitar.Identity.Core.Payloads;

public record RoleModification
{
  public RoleModification() : this(string.Empty, default)
  {
  }
  public RoleModification(string role, CollectionAction action)
  {
    Role = role;
    Action = action;
  }

  public string Role { get; set; }
  public CollectionAction Action { get; set; }
}
