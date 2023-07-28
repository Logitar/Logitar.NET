using Logitar.Identity.Core.ApiKeys.Models;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

public record DeleteApiKeyCommand : IRequest<ApiKey?>
{
  public DeleteApiKeyCommand(string id)
  {
    Id = id;
  }

  public string Id { get; }
}
