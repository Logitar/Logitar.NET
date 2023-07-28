using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

public record CreateApiKeyCommand : IRequest<ApiKey>
{
  public CreateApiKeyCommand(CreateApiKeyPayload payload)
  {
    Payload = payload;
  }

  public CreateApiKeyPayload Payload { get; }
}
