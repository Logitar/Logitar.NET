using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Core.Roles;

public interface IRoleQuerier
{
  Task<Role> ReadAsync(RoleAggregate role, CancellationToken cancellationToken = default);
  Task<Role?> ReadAsync(string id, CancellationToken cancellationToken = default);
  Task<Role?> ReadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken = default);
  Task<SearchResults<Role>> SearchAsync(SearchRolePayload payload, CancellationToken cancellationToken = default);
}
