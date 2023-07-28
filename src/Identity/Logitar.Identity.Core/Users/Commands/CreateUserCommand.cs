using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public record CreateUserCommand : IRequest<User>
{
  public CreateUserCommand(CreateUserPayload payload)
  {
    Payload = payload;
  }

  public CreateUserPayload Payload { get; }
}
