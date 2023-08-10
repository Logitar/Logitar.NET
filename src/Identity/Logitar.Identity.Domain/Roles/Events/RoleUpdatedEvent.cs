using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Roles.Events;

public record RoleUpdatedEvent : DomainEvent, INotification
{
  public string? UniqueName { get; set; }
  public MayBe<string>? DisplayName { get; set; }
  public MayBe<string>? Description { get; set; }

  public Dictionary<string, string?> CustomAttributes { get; init; } = new();
}
