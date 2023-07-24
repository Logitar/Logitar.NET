using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;

namespace Logitar.Identity.Core.Sessions;

public interface ISessionService
{
  Task<Session> CreateAsync(CreateSessionPayload payload, CancellationToken cancellationToken = default);
  Task<Session> SignInAsync(SignInPayload payload, CancellationToken cancellationToken = default);
}
