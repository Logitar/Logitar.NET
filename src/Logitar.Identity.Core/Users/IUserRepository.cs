using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Users.Contact;
using Logitar.Identity.Core.Users.Models;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// Represents an interface that allows retrieving and storing users in an event store.
/// </summary>
public interface IUserRepository
{
  /// <summary>
  /// Loads an user from the event store.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the user belongs to.</param>
  /// <param name="uniqueName">The unique name of the user.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The loaded user.</returns>
  Task<UserAggregate?> LoadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a list of users from the event store.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the users belongs to.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The loaded users.</returns>
  Task<IEnumerable<RoleAggregate>> LoadAsync(string? tenantId, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a list of users from the event store.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the users belongs to.</param>
  /// <param name="email">The email address of the user.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The loaded users.</returns>
  Task<IEnumerable<UserAggregate>> LoadAsync(string? tenantId, ReadOnlyEmail email, CancellationToken cancellationToken = default);

  /// <summary>
  /// Persists an user to the event store.
  /// </summary>
  /// <param name="user">The user to persist.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(UserAggregate user, CancellationToken cancellationToken = default);
  /// <summary>
  /// Persists a list of users to the event store.
  /// </summary>
  /// <param name="users">The list of users to persist.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(IEnumerable<UserAggregate> users, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves the read model of the specified user.
  /// </summary>
  /// <param name="user">The user to retrieve its read model.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The read model.</returns>
  Task<User> ReadAsync(UserAggregate user, CancellationToken cancellationToken = default);
}
