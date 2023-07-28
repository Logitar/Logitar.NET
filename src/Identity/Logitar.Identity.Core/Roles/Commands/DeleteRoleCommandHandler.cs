using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Core.Roles.Commands;

public class DeleteRoleCommandHandler : IDeleteRoleCommand
{
  private readonly IApiKeyRepository _apiKeyRepository;

  public DeleteRoleCommandHandler(IApiKeyRepository apiKeyRepository)
  {
    _apiKeyRepository = apiKeyRepository;
  }

  public async Task ExecuteAsync(RoleAggregate role, CancellationToken cancellationToken)
  {
    IEnumerable<ApiKeyAggregate> apiKeys = await _apiKeyRepository.LoadAsync(role, cancellationToken);
    foreach (ApiKeyAggregate apiKey in apiKeys)
    {
      apiKey.RemoveRole(role);
    }

    await _apiKeyRepository.SaveAsync(apiKeys, cancellationToken);
  }
}
