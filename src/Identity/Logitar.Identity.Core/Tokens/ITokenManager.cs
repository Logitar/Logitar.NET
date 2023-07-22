namespace Logitar.Identity.Core.Tokens;

public interface ITokenManager
{
  string Create(ClaimsIdentity subject, string secret, string? algorithm = null,
    DateTime? expires = null, string? audience = null, string? issuer = null);
}
