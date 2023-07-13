using Logitar.EventSourcing;

namespace Logitar.Identity.Core.ApiKeys.Events;

/// <summary>
/// The event raised when an <see cref="ApiKeyAggregate"/> is authenticated.
/// </summary>
public record ApiKeyAuthenticatedEvent : DomainEvent;
