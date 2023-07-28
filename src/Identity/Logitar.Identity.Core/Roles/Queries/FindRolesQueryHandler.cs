using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Core.Roles.Queries;

public class FindRolesQueryHandler : IFindRolesQuery
{
  private readonly IRoleRepository _roleRepository;

  public FindRolesQueryHandler(IRoleRepository roleRepository)
  {
    _roleRepository = roleRepository;
  }

  public async Task<IEnumerable<RoleAggregate>> ExecuteAsync(string? tenantId,
    IEnumerable<string> ids, string propertyName, CancellationToken cancellationToken)
  {
    int count = ids.Count();
    if (count == 0)
    {
      return Enumerable.Empty<RoleAggregate>();
    }

    List<RoleAggregate> roles = new(capacity: count);
    List<string> missing = new(capacity: count);

    IEnumerable<RoleAggregate> tenantRoles = await _roleRepository.LoadAsync(tenantId, cancellationToken);
    Dictionary<string, RoleAggregate> rolesById = tenantRoles.ToDictionary(x => x.Id.Value, x => x);
    Dictionary<string, RoleAggregate> rolesByUniqueName = tenantRoles.ToDictionary(x => x.UniqueName.ToUpper(), x => x);
    foreach (string id in ids)
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
      throw new RolesNotFoundException(missing, propertyName);
    }

    return roles.AsReadOnly();
  }
}
