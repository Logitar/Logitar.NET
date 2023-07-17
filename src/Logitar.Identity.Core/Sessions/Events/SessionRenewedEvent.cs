using Logitar.EventSourcing;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Core.Sessions.Events;

/// <summary>
/// The event raised when a <see cref="SessionAggregate"/> is renewed.
/// </summary>
public record SessionRenewedEvent : DomainEvent
{
  /// <summary>
  /// Gets or sets the persistence token of the session.
  /// </summary>
  public Pbkdf2 Secret { get; init; } = new(string.Empty);
}
