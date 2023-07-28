using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public record AuthenticateUserCommand : IRequest<User>
{
  public AuthenticateUserCommand(AuthenticateUserPayload payload)
  {
    Payload = payload;
  }

  public AuthenticateUserPayload Payload { get; }
}
