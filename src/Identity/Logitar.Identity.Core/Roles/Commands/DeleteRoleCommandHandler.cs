using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Domain.Roles;
using MediatR;

namespace Logitar.Identity.Core.Roles.Commands;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Role?>
{
  private readonly IMediator _mediator;
  private readonly IRoleQuerier _roleQuerier;
  private readonly IRoleRepository _roleRepository;

  public DeleteRoleCommandHandler(IMediator mediator, IRoleQuerier roleQuerier,
    IRoleRepository roleRepository)
  {
    _mediator = mediator;
    _roleQuerier = roleQuerier;
    _roleRepository = roleRepository;
  }

  public async Task<Role?> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
  {
    AggregateId roleId = command.Id.GetAggregateId(nameof(command.Id));
    RoleAggregate? role = await _roleRepository.LoadAsync(roleId, cancellationToken);
    if (role == null)
    {
      return null;
    }
    Role result = await _roleQuerier.ReadAsync(role, cancellationToken);

    await _mediator.Publish(new DeleteRoleAssociationsCommand(role), cancellationToken);

    role.Delete();

    await _roleRepository.SaveAsync(role, cancellationToken);

    return result;
  }
}
