using Logitar.EventSourcing;
using Logitar.Identity.Core.Users.Models;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

/// <summary>
/// Represents the handler for instances of <see cref="DeleteUserCommand"/>.
/// </summary>
internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, User?>
{
  /// <summary>
  /// The user repository.
  /// </summary>
  private readonly IUserRepository _userRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="DeleteUserCommandHandler"/> class.
  /// </summary>
  /// <param name="userRepository">The user repository.</param>
  public DeleteUserCommandHandler(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  /// <summary>
  /// Handles the specified request.
  /// </summary>
  /// <param name="request">The request to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The resulting read model.</returns>
  public async Task<User?> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
  {
    AggregateId id = request.Id.RequireAggregateId(nameof(request.Id));
    UserAggregate? user = await _userRepository.LoadAsync(id, cancellationToken);
    if (user == null)
    {
      return null;
    }
    User result = await _userRepository.ReadAsync(user, cancellationToken);

    // TODO(fpion): delete user sessions

    user.Delete();

    await _userRepository.SaveAsync(user, cancellationToken);

    return result;
  }
}
