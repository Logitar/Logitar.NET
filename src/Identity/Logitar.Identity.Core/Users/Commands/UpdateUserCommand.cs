using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public record UpdateUserCommand : IRequest<User?>
{
  public UpdateUserCommand(string id, UpdateUserPayload payload)
  {
    Id = id;
    Payload = payload;
  }

  public string Id { get; }
  public UpdateUserPayload Payload { get; }
}
