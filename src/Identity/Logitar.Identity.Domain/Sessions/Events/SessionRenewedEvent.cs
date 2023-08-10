using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using MediatR;

namespace Logitar.Identity.Domain.Sessions.Events;

public record SessionRenewedEvent(Password Secret) : DomainEvent, INotification;
