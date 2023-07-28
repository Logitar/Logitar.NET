using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Commands;

public record CreateSessionCommand : IRequest<Session>
{
  public CreateSessionCommand(CreateSessionPayload payload)
  {
    Payload = payload;
  }

  public CreateSessionPayload Payload { get; }
}
