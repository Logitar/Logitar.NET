namespace Logitar.Identity.Domain.Roles;

public interface IRoleManager
{
  Task DeleteAsync(RoleAggregate role, CancellationToken cancellationToken = default);
  Task SaveAsync(RoleAggregate role, CancellationToken cancellationToken = default);
}
