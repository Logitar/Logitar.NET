using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;

namespace Logitar.Identity.Core.Users;

public interface IUserService
{
  Task<User> AuthenticateAsync(AuthenticateUserPayload payload, CancellationToken cancellationToken = default);
  Task<User?> ChangePasswordAsync(string id, ChangePasswordPayload payload, CancellationToken cancellationToken = default);
  Task<User> CreateAsync(CreateUserPayload payload, CancellationToken cancellationToken = default);
  Task<User?> DeleteAsync(string id, CancellationToken cancellationToken = default);
  Task<User?> ReadAsync(string? id = null, string? tenantId = null, string? uniqueName = null, CancellationToken cancellationToken = default);
  Task<User?> UpdateAsync(string id, UpdateUserPayload payload, CancellationToken cancellationToken = default);
}
