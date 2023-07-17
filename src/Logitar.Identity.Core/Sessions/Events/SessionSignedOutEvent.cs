using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Sessions.Events;

/// <summary>
/// The event raised when a <see cref="SessionAggregate"/> is signed-out.
/// </summary>
public record SessionSignedOutEvent : DomainEvent;
