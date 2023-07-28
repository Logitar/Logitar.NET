using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Roles.Commands;

public record UpdateRoleCommand : IRequest<Role?>
{
  public UpdateRoleCommand(string id, UpdateRolePayload payload)
  {
    Id = id;
    Payload = payload;
  }

  public string Id { get; }
  public UpdateRolePayload Payload { get; }
}
