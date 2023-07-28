using Logitar.Identity.Domain.Users;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Commands;

public record DeleteUserSessionsCommand : IRequest
{
  public DeleteUserSessionsCommand(UserAggregate user)
  {
    User = user;
  }

  public UserAggregate User { get; }
}
