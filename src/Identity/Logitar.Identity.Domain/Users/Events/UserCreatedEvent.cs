using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserCreatedEvent : DomainEvent, INotification
{
  public string? TenantId { get; init; }

  public string UniqueName { get; init; } = string.Empty;
}
