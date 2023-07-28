using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Core.Models;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Queries;

public record SearchApiKeyQuery : IRequest<SearchResults<ApiKey>>
{
  public SearchApiKeyQuery(SearchApiKeyPayload payload)
  {
    Payload = payload;
  }

  public SearchApiKeyPayload Payload { get; }
}
