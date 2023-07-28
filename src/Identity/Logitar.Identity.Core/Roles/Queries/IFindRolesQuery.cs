using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Core.Roles.Queries;

public interface IFindRolesQuery
{
  Task<IEnumerable<RoleAggregate>> ExecuteAsync(string? tenantId, IEnumerable<string> ids, string propertyName, CancellationToken cancellationToken = default);
}
