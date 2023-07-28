using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

public record ReplaceApiKeyCommand : IRequest<ApiKey?>
{
  public ReplaceApiKeyCommand(string id, ReplaceApiKeyPayload payload)
  {
    Id = id;
    Payload = payload;
  }

  public string Id { get; }
  public ReplaceApiKeyPayload Payload { get; }
}
