using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public record ChangePasswordCommand : IRequest<User?>
{
  public ChangePasswordCommand(string id, ChangePasswordPayload payload)
  {
    Id = id;
    Payload = payload;
  }

  public string Id { get; }
  public ChangePasswordPayload Payload { get; }
}
