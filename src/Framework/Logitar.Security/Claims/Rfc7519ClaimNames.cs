namespace Logitar.Security.Claims;

/// <summary>
/// Defines constants for the well-known claim names that can be assigned to a subject.
/// Reference: https://www.iana.org/assignments/jwt/jwt.xhtml
/// </summary>
public static class Rfc7519ClaimNames
{
  /// <summary>
  /// The name of the claim that specifies a postal address.
  /// </summary>
  public const string Address = "address";
  /// <summary>
  /// The name of the claim that specifies an audience.
  /// </summary>
  public const string Audience = "aud";
  /// <summary>
  /// The name of the claim that specifies an authentication time.
  /// </summary>
  public const string AuthenticationTime = "auth_time";
  /// <summary>
  /// The name of the claim that specifies a birthdate.
  /// </summary>
  public const string Birthdate = "birthdate";
  /// <summary>
  /// The name of the claim that specifies an email address.
  /// </summary>
  public const string EmailAddress = "email";
  /// <summary>
  /// The name of the claim that specifies an expiration time.
  /// </summary>
  public const string ExpirationTime = "exp";
  /// <summary>
  /// The name of the claim that specifies a first name.
  /// </summary>
  public const string FirstName = "given_name";
  /// <summary>
  /// The name of the claim that specifies a full name.
  /// </summary>
  public const string FullName = "name";
  /// <summary>
  /// The name of the claim that specifies a gender.
  /// </summary>
  public const string Gender = "gender";
  /// <summary>
  /// The name of the claim that specifies whether an email address is verified or not.
  /// </summary>
  public const string IsEmailVerified = "email_verified";
  /// <summary>
  /// The name of the claim that specifies whether a phone number is verified or not.
  /// </summary>
  public const string IsPhoneVerified = "phone_number_verified";
  /// <summary>
  /// The name of the claim that specifies an issuing time.
  /// </summary>
  public const string IssuedAt = "iat";
  /// <summary>
  /// The name of the claim that specifies an issuer.
  /// </summary>
  public const string Issuer = "iss";
  /// <summary>
  /// The name of the claim that specifies a last name.
  /// </summary>
  public const string LastName = "family_name";
  /// <summary>
  /// The name of the claim that specifies a locale.
  /// </summary>
  public const string Locale = "locale";
  /// <summary>
  /// The name of the claim that specifies a middle name.
  /// </summary>
  public const string MiddleName = "middle_name";
  /// <summary>
  /// The name of the claim that specifies a nickname.
  /// </summary>
  public const string Nickname = "nickname";
  /// <summary>
  /// The name of the claim that specifies a not-before time.
  /// </summary>
  public const string NotBefore = "nbf";
  /// <summary>
  /// The name of the claim that specifies a phone number.
  /// </summary>
  public const string PhoneNumber = "phone_number";
  /// <summary>
  /// The name of the claim that specifies a profile picture URL.
  /// </summary>
  public const string Picture = "picture";
  /// <summary>
  /// The name of the claim that specifies a profile page URL.
  /// </summary>
  public const string Profile = "profile";
  /// <summary>
  /// The name of the claim that specifies a list of roles.
  /// </summary>
  public const string Roles = "roles";
  /// <summary>
  /// The name of the claim that specifies a session identifier.
  /// </summary>
  public const string SessionId = "sid";
  /// <summary>
  /// The name of the claim that specifies a subject.
  /// </summary>
  public const string Subject = "sub";
  /// <summary>
  /// The name of the claim that specifies a time zone.
  /// </summary>
  public const string TimeZone = "zoneinfo";
  /// <summary>
  /// The name of the claim that specifies a token identifier.
  /// </summary>
  public const string TokenId = "jti";
  /// <summary>
  /// The name of the claim that specifies a last update time.
  /// </summary>
  public const string UpdatedAt = "updated_at";
  /// <summary>
  /// The name of the claim that specifies an username.
  /// </summary>
  public const string Username = "preferred_username";
  /// <summary>
  /// The name of the claim that specifies a web page or blog URL.
  /// </summary>
  public const string Website = "website";
}
