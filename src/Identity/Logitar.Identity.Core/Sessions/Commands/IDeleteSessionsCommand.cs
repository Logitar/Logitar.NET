using Logitar.Identity.Domain.Users;

namespace Logitar.Identity.Core.Sessions.Commands;

public interface IDeleteSessionsCommand
{
  Task ExecuteAsync(UserAggregate user, CancellationToken cancellationToken = default);
}
