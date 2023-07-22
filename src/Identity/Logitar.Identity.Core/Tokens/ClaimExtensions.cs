namespace Logitar.Identity.Core.Tokens;

/// <summary>
/// TODO(fpion): move to Logitar.Security.Claims
/// </summary>
public static class ClaimExtensions
{
  public static Claim CreateClaim(this DateTime moment, string type)
    => new(type, new DateTimeOffset(moment).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64);

  public static DateTime ParseDateTime(this Claim claim)
    => DateTimeOffset.FromUnixTimeSeconds(long.Parse(claim.Value)).UtcDateTime;
}
