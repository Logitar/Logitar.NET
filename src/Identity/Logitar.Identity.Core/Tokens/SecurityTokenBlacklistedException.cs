using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.Core.Tokens;

public class SecurityTokenBlacklistedException : SecurityTokenValidationException
{
  public SecurityTokenBlacklistedException(IEnumerable<Guid> blacklistedIds)
    : base(BuildMessage(blacklistedIds))
  {
    BlacklistedIds = blacklistedIds;
  }

  public IEnumerable<Guid> BlacklistedIds
  {
    get => (IEnumerable<Guid>)Data[nameof(BlacklistedIds)]!;
    private set => Data[nameof(BlacklistedIds)] = value;
  }

  private static string BuildMessage(IEnumerable<Guid> blacklistedIds)
  {
    StringBuilder message = new();

    message.AppendLine("The security token is blacklisted.");
    message.AppendLine("Blacklisted identifiers:");
    foreach (Guid blacklistedId in blacklistedIds)
    {
      message.Append(" - ").Append(blacklistedId).AppendLine();
    }

    return message.ToString();
  }
}
