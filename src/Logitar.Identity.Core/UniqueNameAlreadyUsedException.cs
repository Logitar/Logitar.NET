using Logitar.EventSourcing;

namespace Logitar.Identity.Core;

/// <summary>
/// The exception that is thrown when the specified unique name is already used in the specified tenant.
/// </summary>
public class UniqueNameAlreadyUsedException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UniqueNameAlreadyUsedException"/> class.
  /// </summary>
  /// <param name="type">The type of the conflicting aggregate.</param>
  /// <param name="tenantId">The identifier of the tenant in which the unique name is already used.</param>
  /// <param name="uniqueName">The conflicting unique name.</param>
  /// <param name="propertyName">The name of the conflicting property.</param>
  /// <exception cref="ArgumentException">The type is not a subclass of the <see cref="AggregateRoot"/> type.</exception>
  public UniqueNameAlreadyUsedException(Type type, string? tenantId, string uniqueName, string propertyName)
    : base(BuildMessage(type, tenantId, uniqueName, propertyName))
  {
    if (!type.IsSubclassOf(typeof(AggregateRoot)))
    {
      throw new ArgumentException($"The type must be a subclass of the {nameof(AggregateRoot)} type.", nameof(type));
    }

    Data[nameof(Type)] = type.GetName();
    Data[nameof(TenantId)] = tenantId;
    Data[nameof(UniqueName)] = uniqueName;
    Data[nameof(PropertyName)] = propertyName;
  }

  /// <summary>
  /// Gets the type of the conflicting aggregate.
  /// </summary>
  public string Type => (string)Data[nameof(Type)]!;
  /// <summary>
  /// Gets the identifier of the tenant in which the unique name is already used.
  /// </summary>
  public string? TenantId => (string?)Data[nameof(TenantId)];
  /// <summary>
  /// Gets the conflicting unique name.
  /// </summary>
  public string UniqueName => (string)Data[nameof(UniqueName)]!;
  /// <summary>
  /// Gets the name of the conflicting property.
  /// </summary>
  public string PropertyName => (string)Data[nameof(PropertyName)]!;

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="type">The type of the conflicting aggregate.</param>
  /// <param name="tenantId">The identifier of the tenant in which the unique name is already used.</param>
  /// <param name="uniqueName">The conflicting unique name.</param>
  /// <param name="propertyName">The name of the conflicting property.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(Type type, string? tenantId, string uniqueName, string propertyName)
  {
    StringBuilder message = new();

    message.AppendLine("The specified unique name is already used.");
    message.Append("Type: ").AppendLine(type.GetName());
    message.Append("TenantId: ").AppendLine(tenantId);
    message.Append("UniqueName: ").AppendLine(uniqueName);
    message.Append("PropertyName: ").AppendLine(propertyName);

    return message.ToString();
  }
}

/// <summary>
/// The typed exception that is thrown when the specified unique name is already used in the specified tenant.
/// </summary>
/// <typeparam name="T">The type of the conflicting aggregate.</typeparam>
public class UniqueNameAlreadyUsedException<T> : UniqueNameAlreadyUsedException
  where T : AggregateRoot
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UniqueNameAlreadyUsedException{T}"/> class.
  /// </summary>
  /// <param name="tenantId">The identifier of the tenant in which the unique name is already used.</param>
  /// <param name="uniqueName">The attempted unique name.</param>
  /// <param name="propertyName">The name of the property.</param>
  public UniqueNameAlreadyUsedException(string? tenantId, string uniqueName, string propertyName)
    : base(typeof(T), tenantId, uniqueName, propertyName)
  {
  }
}
