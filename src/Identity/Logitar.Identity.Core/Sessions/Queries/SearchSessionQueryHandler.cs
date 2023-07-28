using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Sessions.Models;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Queries;

public class SearchSessionQueryHandler : IRequestHandler<SearchSessionQuery, SearchResults<Session>>
{
  private readonly ISessionQuerier _sessionQuerier;

  public SearchSessionQueryHandler(ISessionQuerier sessionQuerier)
  {
    _sessionQuerier = sessionQuerier;
  }

  public async Task<SearchResults<Session>> Handle(SearchSessionQuery query, CancellationToken cancellationToken)
  {
    return await _sessionQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
