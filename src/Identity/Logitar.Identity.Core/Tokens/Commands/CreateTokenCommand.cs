using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Tokens.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Tokens.Commands;

public record CreateTokenCommand : IRequest<CreatedToken>
{
  public CreateTokenCommand(CreateTokenPayload payload)
  {
    Payload = payload;
  }

  public CreateTokenPayload Payload { get; }
}
