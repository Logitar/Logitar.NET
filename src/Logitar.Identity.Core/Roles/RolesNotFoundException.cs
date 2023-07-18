namespace Logitar.Identity.Core.Roles;

/// <summary>
/// The exception that is thrown when one <see cref="RoleAggregate"/> or more are not found.
/// </summary>
public class RolesNotFoundException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RolesNotFoundException"/> class.
  /// </summary>
  /// <param name="identifiers">The identifiers of the missing roles.</param>
  /// <param name="propertyName">The name of the property name.</param>
  public RolesNotFoundException(IEnumerable<string> identifiers, string propertyName)
    : base(BuildMessage(identifiers, propertyName))
  {
    Data[nameof(Identifiers)] = identifiers;
    Data[nameof(PropertyName)] = propertyName;
  }

  /// <summary>
  /// Gets the identifiers of the missing roles.
  /// </summary>
  public IEnumerable<string> Identifiers => (IEnumerable<string>)Data[nameof(Identifiers)]!;
  /// <summary>
  /// Gets the name of the property.
  /// </summary>
  public string PropertyName => (string)Data[nameof(PropertyName)]!;

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="identifiers">The identifiers of the missing roles.</param>
  /// <param name="propertyName">The name of the property name.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(IEnumerable<string> identifiers, string propertyName)
  {
    StringBuilder message = new();

    message.AppendLine("The specified roles could not be found.");
    message.Append("PropertyName: ").AppendLine(propertyName);

    message.AppendLine("Identifiers:");
    foreach (string identifier in identifiers)
    {
      message.Append(" - ").AppendLine(identifier);
    }

    return message.ToString();
  }
}
