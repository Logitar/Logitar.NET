using Logitar.Identity.Core.Users.Contact;

namespace Logitar.Identity.Core.Users;

/// <summary>
/// The exception that is thrown when the specified email address is already used in the specified tenant.
/// </summary>
public class EmailAddressAlreadyUsedException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="EmailAddressAlreadyUsedException"/> class.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the unique name is already used.</param>
  /// <param name="email">The conflicting email address.</param>
  /// <param name="propertyName">The name of the conflicting property.</param>
  public EmailAddressAlreadyUsedException(string? tenantId, ReadOnlyEmail email, string propertyName)
    : base(BuildMessage(tenantId, email, propertyName))
  {
    Data[nameof(TenantId)] = tenantId;
    Data[nameof(EmailAddress)] = email.Address;
    Data[nameof(PropertyName)] = propertyName;
  }
  /// <summary>
  /// Gets the identifier of the tenant in which the unique name is already used.
  /// </summary>
  public string? TenantId => (string?)Data[nameof(TenantId)];
  /// <summary>
  /// Gets the conflicting unique name.
  /// </summary>
  public string EmailAddress => (string)Data[nameof(EmailAddress)]!;
  /// <summary>
  /// Gets the name of the conflicting property.
  /// </summary>
  public string PropertyName => (string)Data[nameof(PropertyName)]!;

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the unique name is already used.</param>
  /// <param name="email">The conflicting email address.</param>
  /// <param name="propertyName">The name of the conflicting property.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(string? tenantId, ReadOnlyEmail email, string propertyName)
  {
    StringBuilder message = new();

    message.AppendLine("The specified unique name is already used.");
    message.Append("TenantId: ").AppendLine(tenantId);
    message.Append("EmailAddress: ").AppendLine(email.Address);
    message.Append("PropertyName: ").AppendLine(propertyName);

    return message.ToString();
  }
}
