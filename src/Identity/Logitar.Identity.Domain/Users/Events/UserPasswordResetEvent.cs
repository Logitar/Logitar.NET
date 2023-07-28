using Logitar.EventSourcing;
using Logitar.Security;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserPasswordResetEvent : DomainEvent, INotification
{
  public Pbkdf2 Password { get; init; } = new(string.Empty);
}
