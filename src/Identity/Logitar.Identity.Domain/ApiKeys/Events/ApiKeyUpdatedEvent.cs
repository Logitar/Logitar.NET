using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.ApiKeys.Events;

public record ApiKeyUpdatedEvent : DomainEvent, INotification
{
  public string? Title { get; set; }
  public MayBe<string>? Description { get; set; }
  public DateTime? ExpiresOn { get; set; }

  public Dictionary<string, CollectionAction> Roles { get; init; } = new();
}
