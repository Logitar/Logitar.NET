using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.Core.Tokens;

/// <summary>
/// TODO(fpion): shouldn't this class belong to the Infrastructure layer?
/// TODO(fpion): move to Logitar.Security.Tokens or Claims
/// </summary>
public class JsonWebTokenManager : ITokenManager
{
  private const string DefaultAlgorithm = SecurityAlgorithms.HmacSha256;

  private readonly JwtSecurityTokenHandler _tokenHandler = new();

  public string Create(ClaimsIdentity subject, string secret, string? algorithm, DateTime? expires, string? audience, string? issuer)
  {
    SigningCredentials signingCredentials = new(GetSecurityKey(secret), algorithm ?? DefaultAlgorithm);

    SecurityTokenDescriptor tokenDescriptor = new()
    {
      Audience = audience,
      Expires = expires,
      Issuer = issuer,
      SigningCredentials = signingCredentials,
      Subject = subject
    };

    SecurityToken token = _tokenHandler.CreateToken(tokenDescriptor);

    return _tokenHandler.WriteToken(token);
  }

  private static SecurityKey GetSecurityKey(string secret) => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
}
