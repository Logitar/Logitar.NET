using Logitar.EventSourcing;
using Logitar.Security;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionCreatedEvent : DomainEvent, INotification
{
  public AggregateId UserId { get; init; }

  public Pbkdf2? Secret { get; init; }
}
