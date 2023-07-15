﻿namespace Logitar.Identity.Core.Users.Contact;

/// <summary>
/// TODO(fpion): document
/// </summary>
public record ReadOnlyEmail : ReadOnlyContact
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyEmail"/> class.
  /// </summary>
  /// <param name="address">The email address.</param>
  /// <param name="isVerified">A value indicating whether or not the email is verified.</param>
  public ReadOnlyEmail(string address, bool isVerified = false) : base(isVerified)
  {
    Address = address.Trim();
  }

  /// <summary>
  /// Gets the email address.
  /// </summary>
  public string Address { get; }
}
