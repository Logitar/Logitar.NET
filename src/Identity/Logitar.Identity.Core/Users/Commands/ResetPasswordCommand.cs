using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Users.Commands;

public record ResetPasswordCommand : IRequest<User?>
{
  public ResetPasswordCommand(ResetPasswordPayload payload)
  {
    Payload = payload;
  }

  public ResetPasswordPayload Payload { get; }
}
