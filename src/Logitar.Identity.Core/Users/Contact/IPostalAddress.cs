namespace Logitar.Identity.Core.Users.Contact;

/// <summary>
/// Represents an abstraction of postal address contacts.
/// </summary>
interface IPostalAddress
{
  /// <summary>
  /// Gets the street address. It may contain multiple lines, separated by newlines.
  /// </summary>
  string Street { get; }
  /// <summary>
  /// Gets the locality of the postal address.
  /// </summary>
  string Locality { get; }
  /// <summary>
  /// Gets the country of the postal address.
  /// </summary>
  string Country { get; }
  /// <summary>
  /// Gets the region of the postal address.
  /// </summary>
  string? Region { get; }
  /// <summary>
  /// Gets the postal code of the postal address.
  /// </summary>
  string? PostalCode { get; }
}
