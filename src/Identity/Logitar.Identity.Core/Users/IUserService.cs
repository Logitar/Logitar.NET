using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;

namespace Logitar.Identity.Core.Users;

public interface IUserService
{
  Task<User> CreateAsync(CreateUserPayload payload, CancellationToken cancellationToken = default);
}
