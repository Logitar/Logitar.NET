﻿using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users.Events;

public record UserEnabledEvent : DomainEvent
{
  public UserEnabledEvent(ActorId actorId = default)
  {
    ActorId = actorId;
  }
}
