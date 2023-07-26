using Logitar.Identity.Core.Payloads;

namespace Logitar.Identity.Core.Sessions.Payloads;

public record SessionSortOption : SortOption
{
  public SessionSortOption() : this(default(SessionSort))
  {
  }
  public SessionSortOption(SessionSort field, bool isDescending = false)
    : base(field.ToString(), isDescending)
  {
    Field = field;
  }

  public new SessionSort Field { get; set; }
}
