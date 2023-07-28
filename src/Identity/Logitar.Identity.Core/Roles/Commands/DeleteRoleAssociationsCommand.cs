using Logitar.Identity.Domain.Roles;
using MediatR;

namespace Logitar.Identity.Core.Roles.Commands;

public record DeleteRoleAssociationsCommand : INotification
{
  public DeleteRoleAssociationsCommand(RoleAggregate role)
  {
    Role = role;
  }

  public RoleAggregate Role { get; }
}
