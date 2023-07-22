using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserEnabledEvent : DomainEvent, INotification;
