using FluentValidation;
using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Tokens.Payloads;
using Logitar.Identity.Core.Tokens.Validators;

namespace Logitar.Identity.Core.Tokens;

public class TokenService : ITokenService
{
  private readonly ITokenManager _tokenManager;

  public TokenService(ITokenManager tokenManager)
  {
    _tokenManager = tokenManager;
  }

  public Task<CreatedToken> CreateAsync(CreateTokenPayload payload, CancellationToken cancellationToken)
  {
    new CreateTokenValidator().ValidateAndThrow(payload);

    ClaimsIdentity identity = new();

    DateTime? expiresOn = payload.Lifetime.HasValue
      ? DateTime.UtcNow.AddSeconds(payload.Lifetime.Value) : null;
    if (payload.IsConsumable)
    {
      identity.AddClaim(CreateClaim(Rfc7519ClaimTypes.TokenId, Guid.NewGuid().ToString()));
    }
    if (payload.Purpose != null)
    {
      identity.AddClaim(CreateClaim(Rfc7519ClaimTypes.Purpose, payload.Purpose.ToLower()));
    }

    if (!string.IsNullOrWhiteSpace(payload.Subject))
    {
      identity.AddClaim(CreateClaim(Rfc7519ClaimTypes.Subject, payload.Subject.Trim()));
    }
    if (!string.IsNullOrWhiteSpace(payload.EmailAddress))
    {
      identity.AddClaim(CreateClaim(Rfc7519ClaimTypes.EmailAddress, payload.EmailAddress.Trim()));
    }

    if (payload.Claims != null)
    {
      identity.AddClaims(payload.Claims.Select(CreateClaim));
    }

    string token = _tokenManager.Create(identity, payload.Secret, payload.Algorithm?.CleanTrim(),
      expiresOn, payload.Audience?.CleanTrim(), payload.Issuer?.CleanTrim());
    CreatedToken result = new(token);

    return Task.FromResult(result);
  }

  private static Claim CreateClaim(TokenClaim claim)
    => CreateClaim(claim.Type, claim.Value, claim.ValueType);
  private static Claim CreateClaim(string type, string value, string? valueType = null)
    => new(type.Trim(), value.Trim(), valueType?.CleanTrim());
}
