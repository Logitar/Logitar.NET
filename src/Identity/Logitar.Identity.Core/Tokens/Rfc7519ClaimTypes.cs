namespace Logitar.Identity.Core.Tokens;

/// <summary>
/// TODO(fpion): move to Logitar.Security.Claims
/// </summary>
public static class Rfc7519ClaimTypes
{
  public const string Audience = "aud";
  public const string EmailAddress = "email";
  public const string ExpiresOn = "exp";
  public const string Issuer = "iss";
  public const string Purpose = "purpose";
  public const string Subject = "sub";
  public const string TokenId = "jti";
}
