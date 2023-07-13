using Logitar.EventSourcing;

namespace Logitar.Identity.Core.ApiKeys.Events;

/// <summary>
/// The event raised when an <see cref="ApiKeyAggregate"/> is deleted.
/// </summary>
public record ApiKeyDeletedEvent : DomainEvent
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyDeletedEvent"/> class.
  /// </summary>
  public ApiKeyDeletedEvent()
  {
    DeleteAction = DeleteAction.Delete;
  }
}
