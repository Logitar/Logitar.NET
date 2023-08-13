using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionCreatedEvent : DomainEvent, INotification
{
  public AggregateId UserId { get; init; }

  public Password? Secret { get; init; }
}
