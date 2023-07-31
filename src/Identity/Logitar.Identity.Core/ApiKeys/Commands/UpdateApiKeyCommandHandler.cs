using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Core.Payloads;
using Logitar.Identity.Core.Roles.Queries;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

public class UpdateApiKeyCommandHandler : IRequestHandler<UpdateApiKeyCommand, ApiKey?>
{
  private readonly IApiKeyQuerier _apiKeyQuerier;
  private readonly IApiKeyRepository _apiKeyRepository;
  private readonly IMediator _mediator;

  public UpdateApiKeyCommandHandler(IApiKeyQuerier apiKeyQuerier, IApiKeyRepository apiKeyRepository,
    IMediator mediator)
  {
    _apiKeyQuerier = apiKeyQuerier;
    _apiKeyRepository = apiKeyRepository;
    _mediator = mediator;
  }

  public async Task<ApiKey?> Handle(UpdateApiKeyCommand command, CancellationToken cancellationToken)
  {
    UpdateApiKeyPayload payload = command.Payload;

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
    if (payload.Description != null)
    {
      apiKey.Description = payload.Description.Value;
    }
    if (payload.ExpiresOn.HasValue)
    {
      apiKey.ExpiresOn = payload.ExpiresOn.Value;
    }

    foreach (CustomAttributeModification modification in payload.CustomAttributes)
    {
      if (modification.Value == null)
      {
        apiKey.RemoveCustomAttribute(modification.Key);
      }
      else
      {
        apiKey.SetCustomAttribute(modification.Key, modification.Value);
      }
    }

    IEnumerable<string> roleIds = payload.Roles.Select(role => role.Role);
    IEnumerable<RoleAggregate> roles = await _mediator.Send(new FindRolesQuery(apiKey.TenantId, roleIds, nameof(payload.Roles)), cancellationToken);
    Dictionary<string, RoleAggregate> rolesById = roles.ToDictionary(x => x.Id.Value, x => x);
    Dictionary<string, RoleAggregate> rolesByUniqueName = roles.ToDictionary(x => x.UniqueName.ToUpper(), x => x);
    foreach (RoleModification modification in payload.Roles)
    {
      string roleId = modification.Role.Trim();
      if (!rolesById.TryGetValue(roleId, out RoleAggregate? role))
      {
        string uniqueName = roleId.ToUpper();
        role = rolesByUniqueName[uniqueName];
      }

      switch (modification.Action)
      {
        case CollectionAction.Add:
          apiKey.AddRole(role);
          break;
        case CollectionAction.Remove:
          apiKey.RemoveRole(role);
          break;
      }
    }

    await _apiKeyRepository.SaveAsync(apiKey, cancellationToken);

    return await _apiKeyQuerier.ReadAsync(apiKey, cancellationToken);
  }
}
