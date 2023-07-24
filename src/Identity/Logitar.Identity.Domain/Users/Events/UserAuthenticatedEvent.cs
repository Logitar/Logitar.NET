using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserAuthenticatedEvent : DomainEvent, INotification;
