using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Roles.Events;

public record RoleCreatedEvent : DomainEvent, INotification
{
  public string? TenantId { get; init; }

  public string UniqueName { get; init; } = string.Empty;
}
