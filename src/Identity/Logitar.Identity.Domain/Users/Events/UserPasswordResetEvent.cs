using Logitar.EventSourcing;
using Logitar.Security.Cryptography;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserPasswordResetEvent : DomainEvent, INotification
{
  public Password Password { get; init; } = Password.Default;
}
