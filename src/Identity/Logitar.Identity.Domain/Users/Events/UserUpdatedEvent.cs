using Logitar.EventSourcing;
using Logitar.Security;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserUpdatedEvent : DomainEvent, INotification
{
  public string? UniqueName { get; set; }
  public Pbkdf2? Password { get; set; }

  public MayBe<EmailAddress?>? Email { get; set; }

  public MayBe<string?>? FirstName { get; set; }
  public MayBe<string?>? LastName { get; set; }
  public MayBe<string?>? FullName { get; set; }

  public MayBe<CultureInfo?>? Locale { get; set; }
}
