using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Payloads;
using Logitar.Identity.Core.Roles.Queries;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys;

public class ApiKeyService : IApiKeyService
{
  private readonly IApiKeyQuerier _apiKeyQuerier;
  private readonly IApiKeyRepository _apiKeyRepository;
  private readonly IMediator _mediator;

  public ApiKeyService(IApiKeyQuerier apiKeyQuerier, IApiKeyRepository apiKeyRepository,
    IMediator mediator)
  {
    _apiKeyQuerier = apiKeyQuerier;
    _apiKeyRepository = apiKeyRepository;
    _mediator = mediator;
  }

  public virtual async Task<ApiKey> AuthenticateAsync(AuthenticateApiKeyPayload payload, CancellationToken cancellationToken)
  {
    AggregateId id = payload.Id.GetAggregateId(nameof(payload.Id));
    ApiKeyAggregate apiKey = await _apiKeyRepository.LoadAsync(id, cancellationToken)
      ?? throw new InvalidCredentialsException($"The API key 'Id={payload.Id}' could not be found.");

    apiKey.Authenticate(payload.Secret);

    await _apiKeyRepository.SaveAsync(apiKey, cancellationToken);

    return await _apiKeyQuerier.ReadAsync(apiKey, cancellationToken);
  }

  public virtual async Task<ApiKey> CreateAsync(CreateApiKeyPayload payload, CancellationToken cancellationToken)
  {
    ApiKeyAggregate apiKey = new(payload.Title, payload.TenantId)
    {
      Description = payload.Description,
      ExpiresOn = payload.ExpiresOn
    };

    IEnumerable<RoleAggregate> roles = await _mediator.Send(new FindRolesQuery(apiKey.TenantId, payload.Roles, nameof(payload.Roles)), cancellationToken);
    foreach (RoleAggregate role in roles)
    {
      apiKey.AddRole(role);
    }

    await _apiKeyRepository.SaveAsync(apiKey, cancellationToken);

    ApiKey result = await _apiKeyQuerier.ReadAsync(apiKey, cancellationToken);
    if (apiKey.Secret != null)
    {
      result.Secret = apiKey.Secret;
    }

    return result;
  }

  public virtual async Task<ApiKey?> DeleteAsync(string id, CancellationToken cancellationToken)
  {
    AggregateId apiKeyId = id.GetAggregateId(nameof(id));
    ApiKeyAggregate? apiKey = await _apiKeyRepository.LoadAsync(apiKeyId, cancellationToken);
    if (apiKey == null)
    {
      return null;
    }
    ApiKey result = await _apiKeyQuerier.ReadAsync(apiKey, cancellationToken);

    apiKey.Delete();

    await _apiKeyRepository.SaveAsync(apiKey, cancellationToken);

    return result;
  }

  public virtual async Task<ApiKey?> ReadAsync(string? id, CancellationToken cancellationToken)
  {
    Dictionary<string, ApiKey> apiKeys = new(capacity: 1);

    if (id != null)
    {
      ApiKey? apiKey = await _apiKeyQuerier.ReadAsync(id, cancellationToken);
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

  public virtual async Task<ApiKey?> ReplaceAsync(string id, ReplaceApiKeyPayload payload, CancellationToken cancellationToken)
  {
    AggregateId apiKeyId = id.GetAggregateId(nameof(id));
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

  public virtual async Task<SearchResults<ApiKey>> SearchAsync(SearchApiKeyPayload payload, CancellationToken cancellationToken)
  {
    return await _apiKeyQuerier.SearchAsync(payload, cancellationToken);
  }

  public virtual async Task<ApiKey?> UpdateAsync(string id, UpdateApiKeyPayload payload, CancellationToken cancellationToken)
  {
    AggregateId apiKeyId = id.GetAggregateId(nameof(id));
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
