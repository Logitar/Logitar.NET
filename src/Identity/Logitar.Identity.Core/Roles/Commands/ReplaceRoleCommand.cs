using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Roles.Commands;

public record ReplaceRoleCommand : IRequest<Role?>
{
  public ReplaceRoleCommand(string id, ReplaceRolePayload payload)
  {
    Id = id;
    Payload = payload;
  }

  public string Id { get; }
  public ReplaceRolePayload Payload { get; }
}
