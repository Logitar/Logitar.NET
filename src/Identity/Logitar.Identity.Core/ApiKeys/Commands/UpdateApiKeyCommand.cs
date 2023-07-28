using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

public record UpdateApiKeyCommand : IRequest<ApiKey?>
{
  public UpdateApiKeyCommand(string id, UpdateApiKeyPayload payload)
  {
    Id = id;
    Payload = payload;
  }

  public string Id { get; }
  public UpdateApiKeyPayload Payload { get; }
}
