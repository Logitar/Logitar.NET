using Logitar.Identity.Core.Users.Models;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public record SaveExternalIdentifierCommand : IRequest<User?>
{
  public SaveExternalIdentifierCommand(string id, string key, string value)
  {
    Id = id;
    Key = key;
    Value = value;
  }

  public string Id { get; }
  public string Key { get; }
  public string Value { get; }
}
