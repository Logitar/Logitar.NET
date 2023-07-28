using Logitar.Identity.Domain.Roles;
using MediatR;

namespace Logitar.Identity.Core.Roles.Queries;

public class FindRolesQueryHandler : IRequestHandler<FindRolesQuery, IEnumerable<RoleAggregate>>
{
  private readonly IRoleRepository _roleRepository;

  public FindRolesQueryHandler(IRoleRepository roleRepository)
  {
    _roleRepository = roleRepository;
  }

  public async Task<IEnumerable<RoleAggregate>> Handle(FindRolesQuery query, CancellationToken cancellationToken)
  {
    int count = query.Ids.Count();
    if (count == 0)
    {
      return Enumerable.Empty<RoleAggregate>();
    }

    List<RoleAggregate> roles = new(capacity: count);
    List<string> missing = new(capacity: count);

    IEnumerable<RoleAggregate> tenantRoles = await _roleRepository.LoadAsync(query.TenantId, cancellationToken);
    Dictionary<string, RoleAggregate> rolesById = tenantRoles.ToDictionary(x => x.Id.Value, x => x);
    Dictionary<string, RoleAggregate> rolesByUniqueName = tenantRoles.ToDictionary(x => x.UniqueName.ToUpper(), x => x);
    foreach (string id in query.Ids)
    {
      if (rolesById.TryGetValue(id.Trim(), out RoleAggregate? role)
        || rolesByUniqueName.TryGetValue(id.Trim().ToUpper(), out role))
      {
        roles.Add(role);
      }
      else
      {
        missing.Add(id);
      }
    }

    if (missing.Any())
    {
      throw new RolesNotFoundException(missing, query.PropertyName);
    }

    return roles.AsReadOnly();
  }
}
