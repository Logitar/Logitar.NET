using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;

namespace Logitar.Identity.Core.Users;

public interface IUserService
{
  Task<User> CreateAsync(CreateUserPayload payload, CancellationToken cancellationToken = default);
  Task<User?> DeleteAsync(string id, CancellationToken cancellationToken = default);
  Task<User?> UpdateAsync(string id, UpdateUserPayload payload, CancellationToken cancellationToken = default);
}
