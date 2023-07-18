using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles.Models;

namespace Logitar.Identity.Core.Roles;

/// <summary>
/// Represents an interface that allows retrieving and storing roles in an event store.
/// </summary>
public interface IRoleRepository
{
  /// <summary>
  /// Loads a role from the event store.
  /// </summary>
  /// <param name="id">The identifier of the role.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The loaded role.</returns>
  Task<RoleAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a role from the event store.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the role belongs to.</param>
  /// <param name="uniqueName">The unique name of the role.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The loaded role.</returns>
  Task<RoleAggregate?> LoadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken = default);
  /// <summary>
  /// Loads a list of roles from the event store.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the roles belongs to.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The loaded roles.</returns>
  Task<IEnumerable<RoleAggregate>> LoadAsync(string? tenantId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Persists a role to the event store.
  /// </summary>
  /// <param name="role">The role to persist.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(RoleAggregate role, CancellationToken cancellationToken = default);
  /// <summary>
  /// Persists a list of roles to the event store.
  /// </summary>
  /// <param name="roles">The list of roles to persist.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SaveAsync(IEnumerable<RoleAggregate> roles, CancellationToken cancellationToken = default);

  /// <summary>
  /// Retrieves the read model of the specified role.
  /// </summary>
  /// <param name="role">The role to retrieve its read model.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The read model.</returns>
  Task<Role> ReadAsync(RoleAggregate role, CancellationToken cancellationToken = default);
}
