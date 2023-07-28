using Logitar.Identity.Core.Roles.Models;
using MediatR;

namespace Logitar.Identity.Core.Roles.Queries;

public class ReadRoleQueryHandler : IRequestHandler<ReadRoleQuery, Role?>
{
  private readonly IRoleQuerier _roleQuerier;

  public ReadRoleQueryHandler(IRoleQuerier roleQuerier)
  {
    _roleQuerier = roleQuerier;
  }

  public async Task<Role?> Handle(ReadRoleQuery query, CancellationToken cancellationToken)
  {
    Dictionary<string, Role> roles = new(capacity: 2);

    if (query.Id != null)
    {
      Role? role = await _roleQuerier.ReadAsync(query.Id, cancellationToken);
      if (role != null)
      {
        roles[role.Id] = role;
      }
    }

    if (query.UniqueName != null)
    {
      Role? role = await _roleQuerier.ReadAsync(query.TenantId, query.UniqueName, cancellationToken);
      if (role != null)
      {
        roles[role.Id] = role;
      }
    }

    if (roles.Count > 1)
    {
      throw new TooManyResultsException<Role>(expected: 1, actual: roles.Count);
    }

    return roles.Values.SingleOrDefault();
  }
}
