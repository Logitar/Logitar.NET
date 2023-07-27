using Logitar.Identity.Core.Payloads;

namespace Logitar.Identity.Core.ApiKeys.Payloads;

public record ApiKeySortOption : SortOption
{
  public ApiKeySortOption() : this(default(ApiKeySort))
  {
  }
  public ApiKeySortOption(ApiKeySort field, bool isDescending = false)
    : base(field.ToString(), isDescending)
  {
    Field = field;
  }

  public new ApiKeySort Field { get; }
}
