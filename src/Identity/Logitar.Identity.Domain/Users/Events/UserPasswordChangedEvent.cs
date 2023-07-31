using Logitar.EventSourcing;
using Logitar.Security;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserPasswordChangedEvent : DomainEvent, INotification
{
  public Password Password { get; init; } = Password.Default;
}
