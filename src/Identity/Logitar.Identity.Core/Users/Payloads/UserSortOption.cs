using Logitar.Identity.Core.Payloads;

namespace Logitar.Identity.Core.Users.Payloads;

public record UserSortOption : SortOption
{
  public UserSortOption() : this(default(UserSort))
  {
  }
  public UserSortOption(UserSort field, bool isDescending = false)
    : base(field.ToString(), isDescending)
  {
    Field = field;
  }

  public new UserSort Field { get; set; }
}
