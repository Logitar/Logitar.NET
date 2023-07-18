using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

/// <summary>
/// Represents the handler for instances of <see cref="CreateApiKeyCommand"/>.
/// </summary>
internal class CreateApiKeyCommandHandler : IRequestHandler<CreateApiKeyCommand, ApiKey>
{
  /// <summary>
  /// The API key repository.
  /// </summary>
  private readonly IApiKeyRepository _apiKeyRepository;
  /// <summary>
  /// The role repository.
  /// </summary>
  private readonly IRoleRepository _roleRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="CreateApiKeyCommandHandler"/> class.
  /// </summary>
  /// <param name="apiKeyRepository">The API key repository.</param>
  /// <param name="roleRepository">The role repository.</param>
  public CreateApiKeyCommandHandler(IApiKeyRepository apiKeyRepository, IRoleRepository roleRepository)
  {
    _apiKeyRepository = apiKeyRepository;
    _roleRepository = roleRepository;
  }

  /// <summary>
  /// Handles the specified request.
  /// </summary>
  /// <param name="request">The request to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The resulting read model.</returns>
  /// <exception cref="RolesNotFoundException">One or more roles could not be found.</exception>
  public async Task<ApiKey> Handle(CreateApiKeyCommand request, CancellationToken cancellationToken)
  {
    CreateApiKeyPayload payload = request.Payload;

    AggregateId? id = payload.Id?.ParseAggregateId(nameof(payload.Id));
    ApiKeyAggregate apiKey = new(payload.Title, payload.TenantId, id)
    {
      Description = payload.Description,
      ExpiresOn = payload.ExpiresOn
    };

    if (payload.CustomAttributes?.Any() == true)
    {
      foreach (CustomAttribute customAttribute in payload.CustomAttributes)
      {
        apiKey.SetCustomAttribute(customAttribute.Key, customAttribute.Value);
      }
    }

    if (payload.Roles?.Any() == true)
    {
      Dictionary<string, RoleAggregate> rolesById = new();
      Dictionary<string, RoleAggregate> rolesByUniqueName = new();
      IEnumerable<RoleAggregate> roles = await _roleRepository.LoadAsync(apiKey.TenantId, cancellationToken);
      foreach (RoleAggregate role in roles)
      {
        rolesById[role.Id.Value] = role;
        rolesByUniqueName[role.UniqueName.ToUpper()] = role;
      }

      List<string> missingRoles = new();

      foreach (string idOrUniqueName in payload.Roles)
      {
        _ = rolesById.TryGetValue(idOrUniqueName, out RoleAggregate? role);
        if (role == null)
        {
          _ = rolesByUniqueName.TryGetValue(idOrUniqueName.ToUpper(), out role);
        }

        if (role == null)
        {
          missingRoles.Add(idOrUniqueName);
        }
        else
        {
          apiKey.AddRole(role);
        }
      }

      if (missingRoles.Any())
      {
        throw new RolesNotFoundException(missingRoles, nameof(payload.Roles));
      }
    }

    await _apiKeyRepository.SaveAsync(apiKey, cancellationToken);

    return await _apiKeyRepository.ReadAsync(apiKey, cancellationToken);
  }
}
