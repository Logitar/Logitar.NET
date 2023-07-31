using Logitar.EventSourcing;
using Logitar.Security.Cryptography;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserPasswordResetEvent : DomainEvent, INotification
{
  public UserPasswordResetEvent() : this(Password.Default)
  {
  }
  public UserPasswordResetEvent(Password password)
  {
    Password = password;
  }

  public Password Password { get; init; }
}
