using FluentValidation.Results;
using Logitar.EventSourcing;

namespace Logitar.Identity.Core;

public class UniqueNameAlreadyUsedException : Exception
{
  public const string ErrorMessage = "The specified unique name is already used.";

  public UniqueNameAlreadyUsedException(Type type, string? tenantId, string uniqueName, string propertyName)
    : base(BuildMessage(type, tenantId, uniqueName, propertyName))
  {
    if (!type.IsSubclassOf(typeof(AggregateRoot)))
    {
      throw new ArgumentException($"The type must be a subclass of '{nameof(AggregateRoot)}'.", nameof(type));
    }

    Type = type.GetName();
    TenantId = tenantId;
    UniqueName = uniqueName;
    PropertyName = propertyName;
  }

  public string Type
  {
    get => (string)Data[nameof(Type)]!;
    private set => Data[nameof(Type)] = value;
  }
  public string? TenantId
  {
    get => (string?)Data[nameof(TenantId)];
    private set => Data[nameof(TenantId)] = value;
  }
  public string UniqueName
  {
    get => (string)Data[nameof(UniqueName)]!;
    private set => Data[nameof(UniqueName)] = value;
  }
  public string PropertyName
  {
    get => (string)Data[nameof(PropertyName)]!;
    private set => Data[nameof(PropertyName)] = value;
  }

  public ValidationFailure Failure => new(PropertyName, ErrorMessage, attemptedValue: UniqueName)
  {
    ErrorCode = "UniqueNameAlreadyUsed"
  };

  private static string BuildMessage(Type type, string? tenantId, string uniqueName, string propertyName)
  {
    StringBuilder message = new();

    message.AppendLine(ErrorMessage);
    message.Append("Type: ").AppendLine(type.GetName());
    message.Append("TenantId: ").AppendLine(tenantId);
    message.Append("UniqueName: ").AppendLine(uniqueName);
    message.Append("PropertyName: ").Append(propertyName);

    return message.ToString();
  }
}

public class UniqueNameAlreadyUsedException<T> : UniqueNameAlreadyUsedException
  where T : AggregateRoot
{
  public UniqueNameAlreadyUsedException(string? tenantId, string uniqueName, string propertyName)
    : base(typeof(T), tenantId, uniqueName, propertyName)
  {
  }
}
