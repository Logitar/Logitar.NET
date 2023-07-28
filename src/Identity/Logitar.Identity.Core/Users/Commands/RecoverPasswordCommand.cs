using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Users.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public record RecoverPasswordCommand : IRequest<CreatedToken?>
{
  public RecoverPasswordCommand(RecoverPasswordPayload payload)
  {
    Payload = payload;
  }

  public RecoverPasswordPayload Payload { get; }
}
