using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles.Models;
using MediatR;

namespace Logitar.Identity.Core.Roles.Commands;

/// <summary>
/// Represents the handler for instances of <see cref="DeleteRoleCommand"/>.
/// </summary>
internal class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Role?>
{
  /// <summary>
  /// The role repository.
  /// </summary>
  private readonly IRoleRepository _roleRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="DeleteRoleCommandHandler"/> class.
  /// </summary>
  /// <param name="roleRepository">The role repository.</param>
  public DeleteRoleCommandHandler(IRoleRepository roleRepository)
  {
    _roleRepository = roleRepository;
  }

  /// <summary>
  /// Handles the specified request.
  /// </summary>
  /// <param name="request">The request to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The resulting read model.</returns>
  public async Task<Role?> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
  {
    AggregateId id = request.Id.RequireAggregateId(nameof(request.Id));
    RoleAggregate? role = await _roleRepository.LoadAsync(id, cancellationToken);
    if (role == null)
    {
      return null;
    }
    Role result = await _roleRepository.ReadAsync(role, cancellationToken);

    // TODO(fpion): remove role from API keys
    // TODO(fpion): remove role from users

    role.Delete();

    await _roleRepository.SaveAsync(role, cancellationToken);

    return result;
  }
}
