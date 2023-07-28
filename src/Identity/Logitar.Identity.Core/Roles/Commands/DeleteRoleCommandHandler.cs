using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Core.Roles.Commands;

public class DeleteRoleCommandHandler : IDeleteRoleCommand
{
  private readonly IApiKeyRepository _apiKeyRepository;
  private readonly IUserRepository _userRepository;

  public DeleteRoleCommandHandler(IApiKeyRepository apiKeyRepository, IUserRepository userRepository)
  {
    _apiKeyRepository = apiKeyRepository;
    _userRepository = userRepository;
  }

  public async Task ExecuteAsync(RoleAggregate role, CancellationToken cancellationToken)
  {
    IEnumerable<ApiKeyAggregate> apiKeys = await _apiKeyRepository.LoadAsync(role, cancellationToken);
    foreach (ApiKeyAggregate apiKey in apiKeys)
    {
      apiKey.RemoveRole(role);
    }
    await _apiKeyRepository.SaveAsync(apiKeys, cancellationToken);

    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(role, cancellationToken);
    foreach (UserAggregate user in users)
    {
      user.RemoveRole(role);
    }
    await _userRepository.SaveAsync(users, cancellationToken);
  }
}
