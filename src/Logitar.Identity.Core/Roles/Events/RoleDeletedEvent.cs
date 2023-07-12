using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Roles.Events;

/// <summary>
/// The event raised when a <see cref="RoleAggregate"/> is deleted.
/// </summary>
public record RoleDeletedEvent : DomainEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RoleDeletedEvent"/> class.
  /// </summary>
  public RoleDeletedEvent()
  {
    DeleteAction = DeleteAction.Delete;
  }
}
