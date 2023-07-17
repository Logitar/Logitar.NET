using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when an <see cref="UserAggregate"/> is deleted.
/// </summary>
public record UserDeletedEvent : DomainEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UserDeletedEvent"/> class.
  /// </summary>
  public UserDeletedEvent()
  {
    DeleteAction = DeleteAction.Delete;
  }
}
