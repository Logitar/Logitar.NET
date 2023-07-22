using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Users.Events;

public record UserCreatedEvent : DomainEvent
{
  public string? TenantId { get; init; }

  public string UniqueName { get; init; } = string.Empty;
}
