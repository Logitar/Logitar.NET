using Logitar.Identity.Core.Sessions.Models;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Commands;

public record SignOutCommand : IRequest<Session?>
{
  public SignOutCommand(string id)
  {
    Id = id;
  }

  public string Id { get; }
}
