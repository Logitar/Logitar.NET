using Logitar.Identity.Core.Roles.Commands;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

public class DeleteApiKeyRolesCommandHandler : INotificationHandler<DeleteRoleAssociationsCommand>
{
  private readonly IApiKeyRepository _apiKeyRepository;

  public DeleteApiKeyRolesCommandHandler(IApiKeyRepository apiKeyRepository)
  {
    _apiKeyRepository = apiKeyRepository;
  }

  public async Task Handle(DeleteRoleAssociationsCommand command, CancellationToken cancellationToken)
  {
    RoleAggregate role = command.Role;

    IEnumerable<ApiKeyAggregate> apiKeys = await _apiKeyRepository.LoadAsync(role, cancellationToken);
    foreach (ApiKeyAggregate apiKey in apiKeys)
    {
      apiKey.RemoveRole(role);
    }

    await _apiKeyRepository.SaveAsync(apiKeys, cancellationToken);
  }
}
