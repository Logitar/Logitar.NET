using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;

namespace Logitar.Identity.Core.Sessions;

public interface ISessionFacade
{
  Task<Session> CreateAsync(CreateSessionPayload payload, CancellationToken cancellationToken = default);
  Task<Session?> ReadAsync(string? id = null, CancellationToken cancellationToken = default);
  Task<Session> RenewAsync(RenewSessionPayload payload, CancellationToken cancellationToken = default);
  Task<SearchResults<Session>> SearchAsync(SearchSessionPayload payload, CancellationToken cancellationToken = default);
  Task<Session> SignInAsync(SignInPayload payload, CancellationToken cancellationToken = default);
  Task<Session?> SignOutAsync(string id, CancellationToken cancellationToken = default);
}
