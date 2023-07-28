using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Core.Roles.Queries;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

public class ReplaceApiKeyCommandHandler : IRequestHandler<ReplaceApiKeyCommand, ApiKey?>
{
  private readonly IApiKeyQuerier _apiKeyQuerier;
  private readonly IApiKeyRepository _apiKeyRepository;
  private readonly IMediator _mediator;

  public ReplaceApiKeyCommandHandler(IApiKeyQuerier apiKeyQuerier, IApiKeyRepository apiKeyRepository,
    IMediator mediator)
  {
    _apiKeyQuerier = apiKeyQuerier;
    _apiKeyRepository = apiKeyRepository;
    _mediator = mediator;
  }

  public async Task<ApiKey?> Handle(ReplaceApiKeyCommand command, CancellationToken cancellationToken)
  {
    ReplaceApiKeyPayload payload = command.Payload;

    AggregateId apiKeyId = command.Id.GetAggregateId(nameof(command.Id));
    ApiKeyAggregate? apiKey = await _apiKeyRepository.LoadAsync(apiKeyId, cancellationToken);
    if (apiKey == null)
    {
      return null;
    }

    if (payload.Title != null)
    {
      apiKey.Title = payload.Title;
    }

    apiKey.Description = payload.Description;

    if (payload.ExpiresOn.HasValue)
    {
      apiKey.ExpiresOn = payload.ExpiresOn.Value;
    }

    Dictionary<AggregateId, RoleAggregate> roles = (await _mediator.Send(new FindRolesQuery(apiKey.TenantId, payload.Roles, nameof(payload.Roles)), cancellationToken))
      .ToDictionary(x => x.Id, x => x);
    foreach (AggregateId roleId in apiKey.Roles)
    {
      if (!roles.ContainsKey(roleId))
      {
        apiKey.RemoveRole(roleId);
      }
    }
    foreach (RoleAggregate role in roles.Values)
    {
      apiKey.AddRole(role);
    }

    await _apiKeyRepository.SaveAsync(apiKey, cancellationToken);

    return await _apiKeyQuerier.ReadAsync(apiKey, cancellationToken);
  }
}
