namespace Logitar.EventSourcing;

internal record PersonCreatedEvent(string FullName) : DomainEvent;
