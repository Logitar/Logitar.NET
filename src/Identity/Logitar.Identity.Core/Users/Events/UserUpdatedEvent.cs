using Logitar.EventSourcing;
using Logitar.Security;

namespace Logitar.Identity.Core.Users.Events;

public record UserUpdatedEvent : DomainEvent
{
  public string? UniqueName { get; set; }
  public Pbkdf2? Password { get; set; }

  public MayBe<EmailAddress?>? Email { get; set; }

  public MayBe<string?>? FirstName { get; set; }
  public MayBe<string?>? LastName { get; set; }
  public MayBe<string?>? FullName { get; set; }

  public MayBe<CultureInfo?>? Locale { get; set; }
}
