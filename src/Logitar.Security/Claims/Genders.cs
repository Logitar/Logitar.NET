namespace Logitar.Security.Claims;

/// <summary>
/// Defines constants for the known end-user genders. Other values MAY be used when neither of the defined values are applicable.
/// Reference: https://openid.net/specs/openid-connect-core-1_0.html
/// </summary>
public static class Genders
{
  /// <summary>
  /// The value of the female gender.
  /// </summary>
  public const string Female = "female";
  /// <summary>
  /// The value of the male gender.
  /// </summary>
  public const string Male = "male";
}
