using Logitar.EventSourcing;

namespace Logitar.Demo.Ui.Domain.Events;

public record TodoCreatedEvent(string Text) : DomainEvent;
