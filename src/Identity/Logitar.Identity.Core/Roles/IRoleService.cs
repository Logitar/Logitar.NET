using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;

namespace Logitar.Identity.Core.Roles;

public interface IRoleService
{
  Task<Role> CreateAsync(CreateRolePayload payload, CancellationToken cancellationToken = default);
  Task<Role?> DeleteAsync(string id, CancellationToken cancellationToken = default);
  Task<Role?> ReadAsync(string? id = null, string? tenantId = null, string? uniqueName = null, CancellationToken cancellationToken = default);
  Task<Role?> ReplaceAsync(string id, ReplaceRolePayload payload, CancellationToken cancellationToken = default);
  Task<SearchResults<Role>> SearchAsync(SearchRolePayload payload, CancellationToken cancellationToken = default);
  Task<Role?> UpdateAsync(string id, UpdateRolePayload payload, CancellationToken cancellationToken = default);
}
