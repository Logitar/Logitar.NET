using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Core.Roles.Commands;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Role>
{
  private readonly IRoleQuerier _roleQuerier;
  private readonly IRoleRepository _roleRepository;
  private readonly IOptions<RoleSettings> _roleSettings;

  public CreateRoleCommandHandler(IRoleQuerier roleQuerier, IRoleRepository roleRepository,
    IOptions<RoleSettings> roleSettings)
  {
    _roleQuerier = roleQuerier;
    _roleRepository = roleRepository;
    _roleSettings = roleSettings;
  }

  public async Task<Role> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
  {
    CreateRolePayload payload = command.Payload;

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

    foreach (CustomAttribute customAttribute in payload.CustomAttributes)
    {
      role.SetCustomAttribute(customAttribute.Key, customAttribute.Value);
    }

    await _roleRepository.SaveAsync(role, cancellationToken);

    return await _roleQuerier.ReadAsync(role, cancellationToken);
  }
}
