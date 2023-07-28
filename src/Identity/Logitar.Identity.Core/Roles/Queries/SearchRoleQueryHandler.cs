using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles.Models;
using MediatR;

namespace Logitar.Identity.Core.Roles.Queries;

public class SearchRoleQueryHandler : IRequestHandler<SearchRoleQuery, SearchResults<Role>>
{
  private readonly IRoleQuerier _roleQuerier;

  public SearchRoleQueryHandler(IRoleQuerier roleQuerier)
  {
    _roleQuerier = roleQuerier;
  }

  public async Task<SearchResults<Role>> Handle(SearchRoleQuery query, CancellationToken cancellationToken)
  {
    return await _roleQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
