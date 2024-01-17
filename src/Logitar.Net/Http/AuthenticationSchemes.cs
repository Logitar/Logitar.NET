namespace Logitar.Net.Http;

/// <summary>
/// Defines the most used authentication schemes.
/// </summary>
public static class AuthenticationSchemes
{
  /// <summary>
  /// The BASIC authentication scheme, used when supplying client credentials, such as an identifier and a secret, or an user name and password.
  /// </summary>
  public const string Basic = nameof(Basic);
  /// <summary>
  /// The BEARER authentication scheme, used when supplying a Bearer/Access token.
  /// </summary>
  public const string Bearer = nameof(Bearer);
}
