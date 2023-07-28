using Logitar.Identity.Core.Roles.Models;
using MediatR;

namespace Logitar.Identity.Core.Roles.Commands;

public record DeleteRoleCommand : IRequest<Role?>
{
  public DeleteRoleCommand(string id)
  {
    Id = id;
  }

  public string Id { get; }
}
