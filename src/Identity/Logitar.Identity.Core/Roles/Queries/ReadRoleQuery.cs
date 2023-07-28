using Logitar.Identity.Core.Roles.Models;
using MediatR;

namespace Logitar.Identity.Core.Roles.Queries;

public record ReadRoleQuery : IRequest<Role?>
{
  public ReadRoleQuery(string? id = null, string? tenantId = null, string? uniqueName = null)
  {
    Id = id;
    TenantId = tenantId;
    UniqueName = uniqueName;
  }

  public string? Id { get; }
  public string? TenantId { get; }
  public string? UniqueName { get; }
}
