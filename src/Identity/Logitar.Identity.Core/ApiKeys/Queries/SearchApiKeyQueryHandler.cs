using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.Models;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Queries;

public class SearchApiKeyQueryHandler : IRequestHandler<SearchApiKeyQuery, SearchResults<ApiKey>>
{
  private readonly IApiKeyQuerier _apiKeyQuerier;

  public SearchApiKeyQueryHandler(IApiKeyQuerier apiKeyQuerier)
  {
    _apiKeyQuerier = apiKeyQuerier;
  }

  public async Task<SearchResults<ApiKey>> Handle(SearchApiKeyQuery query, CancellationToken cancellationToken)
  {
    return await _apiKeyQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
