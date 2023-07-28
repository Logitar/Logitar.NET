using Logitar.Identity.Core.ApiKeys.Models;
using Logitar.Identity.Core.ApiKeys.Payloads;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

public record AuthenticateApiKeyCommand : IRequest<ApiKey>
{
  public AuthenticateApiKeyCommand(AuthenticateApiKeyPayload payload)
  {
    Payload = payload;
  }

  public AuthenticateApiKeyPayload Payload { get; }
}
