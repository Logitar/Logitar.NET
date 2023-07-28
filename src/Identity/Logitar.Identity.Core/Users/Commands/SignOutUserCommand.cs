using Logitar.Identity.Core.Users.Models;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public record SignOutUserCommand : IRequest<User?>
{
  public SignOutUserCommand(string id)
  {
    Id = id;
  }

  public string Id { get; }
}
