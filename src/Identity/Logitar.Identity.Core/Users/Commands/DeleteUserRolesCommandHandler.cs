using Logitar.Identity.Core.Roles.Commands;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public class DeleteUserRolesCommandHandler : INotificationHandler<DeleteRoleAssociationsCommand>
{
  private readonly IUserRepository _userRepository;

  public DeleteUserRolesCommandHandler(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task Handle(DeleteRoleAssociationsCommand command, CancellationToken cancellationToken)
  {
    RoleAggregate role = command.Role;

    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(role, cancellationToken);
    foreach (UserAggregate user in users)
    {
      user.RemoveRole(role);
    }

    await _userRepository.SaveAsync(users, cancellationToken);
  }
}
