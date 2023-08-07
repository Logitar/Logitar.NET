using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionCreatedEvent : DomainEvent
{
  public AggregateId UserId { get; init; }

  public Password? Secret { get; init; }
}
