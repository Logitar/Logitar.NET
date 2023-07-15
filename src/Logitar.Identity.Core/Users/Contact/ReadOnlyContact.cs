﻿namespace Logitar.Identity.Core.Users.Contact;

/// <summary>
/// TODO(fpion): document
/// </summary>
public abstract record ReadOnlyContact
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ReadOnlyContact"/> class.
  /// </summary>
  /// <param name="isVerified">A value indicating whether or not the contact is verified.</param>
  public ReadOnlyContact(bool isVerified = false)
  {
    IsVerified = isVerified;
  }

  /// <summary>
  /// Gets a value indicating whether or not the contact is verified.
  /// </summary>
  public bool IsVerified { get; }
}
