using Logitar.EventSourcing;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Settings;
using MediatR;

namespace Logitar.Identity.Core.Roles.Commands;

/// <summary>
/// Represents the handler for instances of <see cref="CreateRoleCommand"/>.
/// </summary>
internal class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Role>
{
  /// <summary>
  /// The application context.
  /// </summary>
  private readonly IApplicationContext _applicationContext;
  /// <summary>
  /// The role repository.
  /// </summary>
  private readonly IRoleRepository _roleRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="CreateRoleCommandHandler"/> class.
  /// </summary>
  /// <param name="applicationContext">The application context.</param>
  /// <param name="roleRepository">The role repository.</param>
  public CreateRoleCommandHandler(IApplicationContext applicationContext, IRoleRepository roleRepository)
  {
    _applicationContext = applicationContext;
    _roleRepository = roleRepository;
  }

  /// <summary>
  /// Handles the specified request.
  /// </summary>
  /// <param name="request">The request to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The resulting read model.</returns>
  /// <exception cref="UniqueNameAlreadyUsedException{RoleAggregate}">The role unique name is already used.</exception>
  public async Task<Role> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
  {
    CreateRolePayload payload = request.Payload;

    if (await _roleRepository.LoadAsync(payload.TenantId, payload.UniqueName, cancellationToken) != null)
    {
      throw new UniqueNameAlreadyUsedException<RoleAggregate>(payload.TenantId, payload.UniqueName, nameof(payload.UniqueName));
    }

    IRoleSettings roleSettings = _applicationContext.RoleSettings;
    IUniqueNameSettings uniqueNameSettings = roleSettings.UniqueNameSettings;

    AggregateId? id = payload.Id?.ParseAggregateId(nameof(payload.Id));
    RoleAggregate role = new(uniqueNameSettings, payload.UniqueName, payload.TenantId, id)
    {
      DisplayName = payload.DisplayName,
      Description = payload.Description
    };

    if (payload.CustomAttributes?.Any() == true)
    {
      foreach (CustomAttribute customAttribute in payload.CustomAttributes)
      {
        role.SetCustomAttribute(customAttribute.Key, customAttribute.Value);
      }
    }

    await _roleRepository.SaveAsync(role, cancellationToken);

    return await _roleRepository.ReadAsync(role, cancellationToken);
  }
}
