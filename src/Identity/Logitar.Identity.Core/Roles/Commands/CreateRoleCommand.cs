using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Roles.Commands;

public record CreateRoleCommand : IRequest<Role>
{
  public CreateRoleCommand(CreateRolePayload payload)
  {
    Payload = payload;
  }

  public CreateRolePayload Payload { get; }
}
