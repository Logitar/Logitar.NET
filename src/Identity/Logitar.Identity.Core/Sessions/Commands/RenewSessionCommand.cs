using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Commands;

public record RenewSessionCommand : IRequest<Session>
{
  public RenewSessionCommand(RenewSessionPayload payload)
  {
    Payload = payload;
  }

  public RenewSessionPayload Payload { get; }
}
