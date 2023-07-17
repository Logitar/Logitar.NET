namespace Logitar.Identity.Core.Users.Contact;

/// <summary>
/// Represents an abstraction of email address contacts.
/// </summary>
public interface IEmailAddress
{
  /// <summary>
  /// Gets the email address.
  /// </summary>
  string Address { get; }
}
