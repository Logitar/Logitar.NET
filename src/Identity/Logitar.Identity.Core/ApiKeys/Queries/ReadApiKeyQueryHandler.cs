using Logitar.Identity.Core.ApiKeys.Models;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Queries;

public class ReadApiKeyQueryHandler : IRequestHandler<ReadApiKeyQuery, ApiKey?>
{
  private readonly IApiKeyQuerier _apiKeyQuerier;

  public ReadApiKeyQueryHandler(IApiKeyQuerier apiKeyQuerier)
  {
    _apiKeyQuerier = apiKeyQuerier;
  }

  public async Task<ApiKey?> Handle(ReadApiKeyQuery query, CancellationToken cancellationToken)
  {
    Dictionary<string, ApiKey> apiKeys = new(capacity: 1);

    if (query.Id != null)
    {
      ApiKey? apiKey = await _apiKeyQuerier.ReadAsync(query.Id, cancellationToken);
      if (apiKey != null)
      {
        apiKeys[apiKey.Id] = apiKey;
      }
    }

    if (apiKeys.Count > 1)
    {
      throw new TooManyResultsException<ApiKey>(expected: 1, actual: apiKeys.Count);
    }

    return apiKeys.Values.SingleOrDefault();
  }
}
