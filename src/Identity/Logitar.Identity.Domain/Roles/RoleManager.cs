using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles.Events;
using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Domain.Roles;

public class RoleManager : IRoleManager
{
  private readonly IAggregateRepository _aggregateRepository;
  private readonly IRoleRepository _roleRepository;
  private readonly IUserRepository _userRepository;

  public RoleManager(IAggregateRepository aggregateRepository, IRoleRepository roleRepository,
    IUserRepository userRepository)
  {
    _aggregateRepository = aggregateRepository;
    _roleRepository = roleRepository;
    _userRepository = userRepository;
  }

  public async Task DeleteAsync(RoleAggregate role, CancellationToken cancellationToken)
  {
    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(role, cancellationToken);
    foreach (UserAggregate user in users)
    {
      user.RemoveRole(role);
    }
    await _aggregateRepository.SaveAsync(users, cancellationToken);

    role.Delete();
    await _aggregateRepository.SaveAsync(role, cancellationToken);
  }

  public async Task SaveAsync(RoleAggregate role, CancellationToken cancellationToken)
  {
    bool hasUniqueNameChanged = false;
    foreach (DomainEvent change in role.Changes)
    {
      if (change is RoleCreatedEvent || (change is RoleUpdatedEvent updated && updated.UniqueName != null))
      {
        hasUniqueNameChanged = true;
        break;
      }
    }
    if (hasUniqueNameChanged)
    {
      RoleAggregate? other = await _roleRepository.LoadAsync(role.TenantId, role.UniqueName, cancellationToken);
      if (other?.Equals(role) == false)
      {
        throw new UniqueNameAlreadyUsedException<RoleAggregate>(role.TenantId, role.UniqueName, nameof(role.UniqueName));
      }
    }

    await _aggregateRepository.SaveAsync(role, cancellationToken);
  }
}
