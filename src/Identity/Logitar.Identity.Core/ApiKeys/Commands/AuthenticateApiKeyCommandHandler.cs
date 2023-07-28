using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.ApiKeys;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

public class AuthenticateApiKeyCommandHandler : IRequestHandler<AuthenticateApiKeyCommand, ApiKey>
{
  private readonly IApiKeyQuerier _apiKeyQuerier;
  private readonly IApiKeyRepository _apiKeyRepository;

  public AuthenticateApiKeyCommandHandler(IApiKeyQuerier apiKeyQuerier, IApiKeyRepository apiKeyRepository)
  {
    _apiKeyQuerier = apiKeyQuerier;
    _apiKeyRepository = apiKeyRepository;
  }

  public async Task<ApiKey> Handle(AuthenticateApiKeyCommand command, CancellationToken cancellationToken)
  {
    AuthenticateApiKeyPayload payload = command.Payload;

    AggregateId id = payload.Id.GetAggregateId(nameof(payload.Id));
    ApiKeyAggregate apiKey = await _apiKeyRepository.LoadAsync(id, cancellationToken)
      ?? throw new InvalidCredentialsException($"The API key 'Id={payload.Id}' could not be found.");

    apiKey.Authenticate(payload.Secret);

    await _apiKeyRepository.SaveAsync(apiKey, cancellationToken);

    return await _apiKeyQuerier.ReadAsync(apiKey, cancellationToken);
  }
}
