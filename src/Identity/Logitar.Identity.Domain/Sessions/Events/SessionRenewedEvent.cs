using Logitar.EventSourcing;
using Logitar.Security.Cryptography;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionRenewedEvent : DomainEvent, INotification
{
  public SessionRenewedEvent() : this(Password.Default)
  {
  }
  public SessionRenewedEvent(Password secret)
  {
    Secret = secret;
  }

  public Password Secret { get; init; }
}
