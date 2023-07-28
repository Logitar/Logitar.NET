using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public record ReplaceUserCommand : IRequest<User?>
{
  public ReplaceUserCommand(string id, ReplaceUserPayload payload)
  {
    Id = id;
    Payload = payload;
  }

  public string Id { get; }
  public ReplaceUserPayload Payload { get; }
}
