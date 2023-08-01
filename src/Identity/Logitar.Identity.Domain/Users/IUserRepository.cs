using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;

namespace Logitar.Identity.Domain.Users;

public interface IUserRepository
{
  Task<UserAggregate?> LoadAsync(AggregateId id, CancellationToken cancellationToken = default);
  Task<UserAggregate?> LoadAsync(string? tenantId, string uniqueName, CancellationToken cancellationToken = default);
  Task<UserAggregate?> LoadAsync(string? tenantId, string externalIdentifierKey, string externalIdentifierValue, CancellationToken cancellationToken = default);
  Task<IEnumerable<UserAggregate>> LoadAsync(string? tenantId, IEmailAddress email, CancellationToken cancellationToken = default);
  Task<IEnumerable<UserAggregate>> LoadAsync(RoleAggregate role, CancellationToken cancellationToken = default);

  Task SaveAsync(UserAggregate user, CancellationToken cancellationToken = default);
  Task SaveAsync(IEnumerable<UserAggregate> users, CancellationToken cancellationToken = default);
}
