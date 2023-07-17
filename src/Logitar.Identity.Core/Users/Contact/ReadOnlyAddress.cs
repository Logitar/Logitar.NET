namespace Logitar.Identity.Core.Users.Contact;

/// <summary>
/// Represents a postal address contact.
/// </summary>
public record ReadOnlyAddress : ReadOnlyContact, IPostalAddress
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyAddress"/> class.
  /// </summary>
  /// <param name="street">The street address.</param>
  /// <param name="locality">The locality of the postal address.</param>
  /// <param name="country">The country of the postal address.</param>
  /// <param name="region">The region of the postal address.</param>
  /// <param name="postalCode">The postal code of the postal address.</param>
  /// <param name="isVerified">A value indicating whether or not the postal is verified.</param>
  public ReadOnlyAddress(string street, string locality, string country,
    string? region = null, string? postalCode = null, bool isVerified = false) : base(isVerified)
  {
    Street = street.Trim();
    Locality = locality.Trim();
    Country = country.Trim();
    Region = region?.CleanTrim();
    PostalCode = postalCode?.CleanTrim();
  }

  /// <summary>
  /// Gets the street address. It may contain multiple lines, separated by newlines.
  /// </summary>
  public string Street { get; }
  /// <summary>
  /// Gets the locality of the postal address.
  /// </summary>
  public string Locality { get; }
  /// <summary>
  /// Gets the country of the postal address.
  /// </summary>
  public string Country { get; }
  /// <summary>
  /// Gets the region of the postal address.
  /// </summary>
  public string? Region { get; }
  /// <summary>
  /// Gets the postal code of the postal address.
  /// </summary>
  public string? PostalCode { get; }
}
