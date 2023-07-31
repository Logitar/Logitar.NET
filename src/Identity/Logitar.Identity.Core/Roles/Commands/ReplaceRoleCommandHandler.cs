using Logitar.EventSourcing;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Roles.Commands;

public class ReplaceRoleCommandHandler : IRequestHandler<ReplaceRoleCommand, Role?>
{
  private readonly IRoleQuerier _roleQuerier;
  private readonly IRoleRepository _roleRepository;
  private readonly IOptions<RoleSettings> _roleSettings;

  public ReplaceRoleCommandHandler(IRoleQuerier roleQuerier, IRoleRepository roleRepository,
    IOptions<RoleSettings> roleSettings)
  {
    _roleQuerier = roleQuerier;
    _roleRepository = roleRepository;
    _roleSettings = roleSettings;
  }

  public async Task<Role?> Handle(ReplaceRoleCommand command, CancellationToken cancellationToken)
  {
    AggregateId roleId = command.Id.GetAggregateId(nameof(command.Id));
    RoleAggregate? role = await _roleRepository.LoadAsync(roleId, cancellationToken);
    if (role == null)
    {
      return null;
    }

    ReplaceRolePayload payload = command.Payload;
    RoleSettings roleSettings = _roleSettings.Value;

    if (payload.UniqueName != null)
    {
      RoleAggregate? other = await _roleRepository.LoadAsync(role.TenantId, payload.UniqueName, cancellationToken);
      if (other?.Equals(role) == false)
      {
        throw new UniqueNameAlreadyUsedException<RoleAggregate>(role.TenantId, payload.UniqueName, nameof(payload.UniqueName));
      }

      role.SetUniqueName(roleSettings.UniqueNameSettings, payload.UniqueName);
    }

    role.DisplayName = payload.DisplayName;
    role.Description = payload.Description;

    HashSet<string> customAttributes = payload.CustomAttributes.Select(c => c.Key).ToHashSet();
    foreach (string key in role.CustomAttributes.Keys)
    {
      if (!customAttributes.Contains(key))
      {
        role.RemoveCustomAttribute(key);
      }
    }
    foreach (CustomAttribute customAttribute in payload.CustomAttributes)
    {
      role.SetCustomAttribute(customAttribute.Key, customAttribute.Value);
    }

    await _roleRepository.SaveAsync(role, cancellationToken);

    return await _roleQuerier.ReadAsync(role, cancellationToken);
  }
}
