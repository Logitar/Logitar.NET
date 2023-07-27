using Logitar.Identity.Core.Payloads;

namespace Logitar.Identity.Core.Roles.Payloads;

public record RoleSortOption : SortOption
{
  public RoleSortOption() : this(default(RoleSort))
  {
  }
  public RoleSortOption(RoleSort field, bool isDescending = false)
    : base(field.ToString(), isDescending)
  {
  }

  public new RoleSort Field { get; set; }
}
