using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserPasswordChangedEvent(Password Password) : DomainEvent, INotification;
