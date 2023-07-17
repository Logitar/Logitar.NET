using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Sessions.Events;

/// <summary>
/// The event raised when a <see cref="SessionAggregate"/> is deleted.
/// </summary>
public record SessionDeletedEvent : DomainEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionDeletedEvent"/> class.
  /// </summary>
  public SessionDeletedEvent()
  {
    DeleteAction = DeleteAction.Delete;
  }
}
