using Logitar.EventSourcing;
using Logitar.Security;
using MediatR;

namespace Logitar.Identity.Domain.ApiKeys.Events;

public record ApiKeyCreatedEvent : DomainEvent, INotification
{
  public Password Secret { get; init; } = Password.Default;

  public string? TenantId { get; init; }

  public string Title { get; init; } = string.Empty;
}
