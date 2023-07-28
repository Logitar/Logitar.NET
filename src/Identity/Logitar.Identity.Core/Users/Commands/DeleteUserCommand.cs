using Logitar.Identity.Core.Users.Models;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public record DeleteUserCommand : IRequest<User?>
{
  public DeleteUserCommand(string id)
  {
    Id = id;
  }

  public string Id { get; }
}
