using Logitar.EventSourcing;
using Logitar.Security.Cryptography;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionRenewedEvent : DomainEvent, INotification
{
  public Password Secret { get; init; } = Password.Default;
}
