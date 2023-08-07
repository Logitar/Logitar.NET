using Logitar.EventSourcing;

namespace Logitar.Identity.Domain.Roles.Events;

public record RoleCreatedEvent : DomainEvent
{
  public string? TenantId { get; init; }

  public string UniqueName { get; init; } = string.Empty;
}
