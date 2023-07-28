using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Roles.Commands;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Role?>
{
  private readonly IRoleQuerier _roleQuerier;
  private readonly IRoleRepository _roleRepository;
  private readonly IOptions<RoleSettings> _roleSettings;

  public UpdateRoleCommandHandler(IRoleQuerier roleQuerier, IRoleRepository roleRepository,
    IOptions<RoleSettings> roleSettings)
  {
    _roleQuerier = roleQuerier;
    _roleRepository = roleRepository;
    _roleSettings = roleSettings;
  }

  public async Task<Role?> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
  {
    AggregateId roleId = command.Id.GetAggregateId(nameof(command.Id));
    RoleAggregate? role = await _roleRepository.LoadAsync(roleId, cancellationToken);
    if (role == null)
    {
      return null;
    }

    UpdateRolePayload payload = command.Payload;
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
