using FluentValidation;
using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Tokens.Payloads;
using Logitar.Identity.Core.Tokens.Validators;
using MediatR;

namespace Logitar.Identity.Core.Tokens.Commands;

public class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, ValidatedToken>
{
  private readonly ITokenManager _tokenManager;

  public ValidateTokenCommandHandler(ITokenManager tokenManager)
  {
    _tokenManager = tokenManager;
  }

  public async Task<ValidatedToken> Handle(ValidateTokenCommand command, CancellationToken cancellationToken)
  {
    ValidateTokenPayload payload = command.Payload;
    new ValidateTokenValidator().ValidateAndThrow(payload);

    ClaimsPrincipal principal = await _tokenManager.ValidateAsync(payload.Token, payload.Secret,
        payload.Audience, payload.Issuer, payload.Purpose, command.Consume, cancellationToken);

    ValidatedToken token = new();

    List<TokenClaim> claims = new(capacity: principal.Claims.Count());
    foreach (Claim claim in principal.Claims)
    {
      switch (claim.Type)
      {
        case Rfc7519ClaimTypes.EmailAddress:
          token.EmailAddress = claim.Value;
          break;
        case Rfc7519ClaimTypes.Subject:
          token.Subject = claim.Value;
          break;
        default:
          claims.Add(new TokenClaim(claim.Type, claim.Value, claim.ValueType));
          break;
      }
    }

    token.Claims = claims;

    return token;
  }
}
