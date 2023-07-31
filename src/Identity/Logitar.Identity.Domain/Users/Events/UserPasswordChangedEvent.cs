﻿using Logitar.EventSourcing;
using Logitar.Security.Cryptography;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserPasswordChangedEvent : DomainEvent, INotification
{
  public UserPasswordChangedEvent() : this(Password.Default)
  {
  }
  public UserPasswordChangedEvent(Password password)
  {
    Password = password;
  }

  public Password Password { get; init; }
}
