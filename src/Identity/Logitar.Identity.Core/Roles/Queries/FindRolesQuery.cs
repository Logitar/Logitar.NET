using Logitar.Identity.Domain.Roles;
using MediatR;

namespace Logitar.Identity.Core.Roles.Queries;

public record FindRolesQuery : IRequest<IEnumerable<RoleAggregate>>
{
  public FindRolesQuery(string? tenantId, IEnumerable<string> ids, string propertyName)
  {
    TenantId = tenantId;
    Ids = ids;
    PropertyName = propertyName;
  }

  public string? TenantId { get; }
  public IEnumerable<string> Ids { get; }
  public string PropertyName { get; }
}
