using Logitar.EventSourcing;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Core.Sessions.Events;

/// <summary>
/// The event raised when a <see cref="SessionAggregate"/> is created.
/// </summary>
public record SessionCreatedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the identifier of the user to whom the session belongs to.
  /// </summary>
  public AggregateId UserId { get; init; }

  /// <summary>
  /// Gets or sets the persistence token of the session.
  /// </summary>
  public Pbkdf2? Secret { get; init; }
}
