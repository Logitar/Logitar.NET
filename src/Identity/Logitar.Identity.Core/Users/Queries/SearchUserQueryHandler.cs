using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Users.Models;
using MediatR;

namespace Logitar.Identity.Core.Users.Queries;

public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, SearchResults<User>>
{
  private readonly IUserQuerier _userQuerier;

  public SearchUserQueryHandler(IUserQuerier userQuerier)
  {
    _userQuerier = userQuerier;
  }

  public async Task<SearchResults<User>> Handle(SearchUserQuery query, CancellationToken cancellationToken)
  {
    return await _userQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
