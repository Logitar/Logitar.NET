using Logitar.EventSourcing;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Roles;

public class RoleService : IRoleService
{
  private readonly IRoleQuerier _roleQuerier;
  private readonly IRoleRepository _roleRepository;
  private readonly IOptions<RoleSettings> _roleSettings;

  public RoleService(IRoleQuerier roleQuerier, IRoleRepository roleRepository,
    IOptions<RoleSettings> roleSettings)
  {
    _roleQuerier = roleQuerier;
    _roleRepository = roleRepository;
    _roleSettings = roleSettings;
  }

  public virtual async Task<Role> CreateAsync(CreateRolePayload payload, CancellationToken cancellationToken)
  {
    if (await _roleRepository.LoadAsync(payload.TenantId, payload.UniqueName, cancellationToken) != null)
    {
      throw new UniqueNameAlreadyUsedException<RoleAggregate>(payload.TenantId, payload.UniqueName, nameof(payload.UniqueName));
    }

    RoleSettings roleSettings = _roleSettings.Value;

    RoleAggregate role = new(roleSettings.UniqueNameSettings, payload.UniqueName, payload.TenantId)
    {
      DisplayName = payload.DisplayName,
      Description = payload.Description
    };

    await _roleRepository.SaveAsync(role, cancellationToken);

    return await _roleQuerier.ReadAsync(role, cancellationToken);
  }

  public virtual async Task<Role?> DeleteAsync(string id, CancellationToken cancellationToken)
  {
    AggregateId roleId = id.GetAggregateId(nameof(id));
    RoleAggregate? role = await _roleRepository.LoadAsync(roleId, cancellationToken);
    if (role == null)
    {
      return null;
    }
    Role result = await _roleQuerier.ReadAsync(role, cancellationToken);

    role.Delete();

    await _roleRepository.SaveAsync(role, cancellationToken);

    return result;
  }

  public virtual async Task<Role?> ReadAsync(string? id, string? tenantId, string? uniqueName, CancellationToken cancellationToken)
  {
    Dictionary<string, Role> roles = new(capacity: 2);

    if (id != null)
    {
      Role? role = await _roleQuerier.ReadAsync(id, cancellationToken);
      if (role != null)
      {
        roles[role.Id] = role;
      }
    }

    if (uniqueName != null)
    {
      Role? role = await _roleQuerier.ReadAsync(tenantId, uniqueName, cancellationToken);
      if (role != null)
      {
        roles[role.Id] = role;
      }
    }

    if (roles.Count > 1)
    {
      throw new TooManyResultsException<Role>(expected: 1, actual: roles.Count);
    }

    return roles.Values.SingleOrDefault();
  }

  public virtual async Task<Role?> ReplaceAsync(string id, ReplaceRolePayload payload, CancellationToken cancellationToken)
  {
    AggregateId roleId = id.GetAggregateId(nameof(id));
    RoleAggregate? role = await _roleRepository.LoadAsync(roleId, cancellationToken);
    if (role == null)
    {
      return null;
    }

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

    await _roleRepository.SaveAsync(role, cancellationToken);

    return await _roleQuerier.ReadAsync(role, cancellationToken);
  }

  public virtual async Task<SearchResults<Role>> SearchAsync(SearchRolePayload payload, CancellationToken cancellationToken)
  {
    return await _roleQuerier.SearchAsync(payload, cancellationToken);
  }

  public virtual async Task<Role?> UpdateAsync(string id, UpdateRolePayload payload, CancellationToken cancellationToken)
  {
    AggregateId roleId = id.GetAggregateId(nameof(id));
    RoleAggregate? role = await _roleRepository.LoadAsync(roleId, cancellationToken);
    if (role == null)
    {
      return null;
    }

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
    if (payload.DisplayName != null)
    {
      role.DisplayName = payload.DisplayName.Value;
    }
    if (payload.Description != null)
    {
      role.Description = payload.Description.Value;
    }

    await _roleRepository.SaveAsync(role, cancellationToken);

    return await _roleQuerier.ReadAsync(role, cancellationToken);
  }
}
