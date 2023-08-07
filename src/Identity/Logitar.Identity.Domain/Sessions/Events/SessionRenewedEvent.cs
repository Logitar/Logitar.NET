using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionRenewedEvent(Password Secret) : DomainEvent;
