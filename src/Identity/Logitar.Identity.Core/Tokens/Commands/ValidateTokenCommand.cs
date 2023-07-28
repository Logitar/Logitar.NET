using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Tokens.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Tokens.Commands;

public record ValidateTokenCommand : IRequest<ValidatedToken>
{
  public ValidateTokenCommand(ValidateTokenPayload payload, bool consume = false)
  {
    Payload = payload;
    Consume = consume;
  }

  public ValidateTokenPayload Payload { get; }
  public bool Consume { get; }
}
