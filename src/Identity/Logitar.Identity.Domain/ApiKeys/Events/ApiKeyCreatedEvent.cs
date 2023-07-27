using Logitar.EventSourcing;
using Logitar.Security;
using MediatR;

namespace Logitar.Identity.Domain.ApiKeys.Events;

public record ApiKeyCreatedEvent : DomainEvent, INotification
{
  public Pbkdf2 Secret { get; init; } = new(string.Empty);

  public string? TenantId { get; init; }

  public string Title { get; init; } = string.Empty;
}
