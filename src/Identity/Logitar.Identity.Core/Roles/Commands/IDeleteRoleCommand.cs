using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Core.Roles.Commands;

public interface IDeleteRoleCommand
{
  Task ExecuteAsync(RoleAggregate role, CancellationToken cancellationToken = default);
}
