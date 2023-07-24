using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Domain.Sessions;

namespace Logitar.Identity.Core.Sessions;

public interface ISessionQuerier
{
  Task<Session> ReadAsync(SessionAggregate session, CancellationToken cancellationToken = default);
  Task<Session?> ReadAsync(string id, CancellationToken cancellationToken = default);
}
