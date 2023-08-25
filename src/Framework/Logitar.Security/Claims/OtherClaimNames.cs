namespace Logitar.Security.Claims;

/// <summary>
/// Defines constants for other useful claim names that can be assigned to a subject.
/// </summary>
public static class OtherClaimNames
{
  /// <summary>
  /// The name of the claim that specifies whether a postal address is verified or not.
  /// </summary>
  public const string IsAddressVerified = "address_verified";
  /// <summary>
  /// The name of the claim that specifies a token purpose.
  /// </summary>
  public const string Purpose = "purpose";
}
