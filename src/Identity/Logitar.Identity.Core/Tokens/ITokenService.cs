using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Tokens.Payloads;

namespace Logitar.Identity.Core.Tokens;

public interface ITokenService
{
  Task<CreatedToken> CreateAsync(CreateTokenPayload payload, CancellationToken cancellationToken = default);
  Task<ValidatedToken> ValidateAsync(ValidateTokenPayload payload, bool consume = false, CancellationToken cancellationToken = default);
}
