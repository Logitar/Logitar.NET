﻿using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Commands;

public record SignInCommand : IRequest<Session>
{
  public SignInCommand(SignInPayload payload)
  {
    Payload = payload;
  }

  public SignInPayload Payload { get; }
}
