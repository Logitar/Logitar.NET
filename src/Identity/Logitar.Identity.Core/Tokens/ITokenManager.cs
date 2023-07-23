namespace Logitar.Identity.Core.Tokens;

/// <summary>
/// TODO(fpion): move to Logitar.Security.Tokens or Claims
/// </summary>
public interface ITokenManager
{
  string Create(ClaimsIdentity subject, string secret, string? algorithm = null,
    DateTime? expires = null, string? audience = null, string? issuer = null);
  Task<ClaimsPrincipal> ValidateAsync(string token, string secret, string? audience = null,
    string? issuer = null, string? purpose = null, bool consume = false,
    CancellationToken cancellationToken = default);
}
