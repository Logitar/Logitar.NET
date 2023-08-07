using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionUpdatedEvent : DomainEvent
{
  public Dictionary<string, string?> CustomAttributes { get; init; } = new();
}
