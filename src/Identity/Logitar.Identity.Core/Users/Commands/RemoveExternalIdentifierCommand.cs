using Logitar.Identity.Core.Users.Models;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public record RemoveExternalIdentifierCommand : IRequest<User?>
{
  public RemoveExternalIdentifierCommand(string id, string key)
  {
    Id = id;
    Key = key;
  }

  public string Id { get; }
  public string Key { get; }
}
