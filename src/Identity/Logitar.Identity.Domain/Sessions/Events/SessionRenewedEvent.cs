using Logitar.EventSourcing;
using Logitar.Security;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionRenewedEvent : DomainEvent, INotification
{
  public Pbkdf2 Secret { get; init; } = new(string.Empty);
}
