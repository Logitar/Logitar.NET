using Logitar.EventSourcing;
using Logitar.Security;
using MediatR;

namespace Logitar.Identity.Domain.Users.Events;

public record UserUpdatedEvent : DomainEvent, INotification
{
  public string? UniqueName { get; set; }
  public Password? Password { get; set; }

  public MayBe<PostalAddress>? Address { get; set; }
  public MayBe<EmailAddress>? Email { get; set; }
  public MayBe<PhoneNumber>? Phone { get; set; }

  public MayBe<string>? FirstName { get; set; }
  public MayBe<string>? MiddleName { get; set; }
  public MayBe<string>? LastName { get; set; }
  public MayBe<string>? FullName { get; set; }
  public MayBe<string>? Nickname { get; set; }

  public MayBe<DateTime?>? Birthdate { get; set; }
  public MayBe<Gender>? Gender { get; set; }
  public MayBe<CultureInfo>? Locale { get; set; }
  public MayBe<TimeZoneEntry>? TimeZone { get; set; }

  public MayBe<Uri>? Picture { get; set; }
  public MayBe<Uri>? Profile { get; set; }
  public MayBe<Uri>? Website { get; set; }

  public Dictionary<string, CollectionAction> Roles { get; init; } = new();
}
