using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Domain.ApiKeys.Events;

public record ApiKeyAuthenticatedEvent : DomainEvent, INotification;
