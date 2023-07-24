using FluentValidation.Results;
using Logitar.EventSourcing;

namespace Logitar.Identity.Core;

public class AggregateNotFoundException : Exception
{
  public const string ErrorMessage = "The specified aggregate could not be found.";

  public AggregateNotFoundException(Type type, string id, string propertyName)
    : base(BuildMessage(type, id, propertyName))
  {
    if (!type.IsSubclassOf(typeof(AggregateRoot)))
    {
      throw new ArgumentException($"The type must be a subclass of '{nameof(AggregateRoot)}'.", nameof(type));
    }

    Type = type.GetName();
    Id = id;
    PropertyName = propertyName;
  }

  public string Type
  {
    get => (string)Data[nameof(Type)]!;
    private set => Data[nameof(Type)] = value;
  }
  public string Id
  {
    get => (string)Data[nameof(Id)]!;
    private set => Data[nameof(Id)] = value;
  }
  public string PropertyName
  {
    get => (string)Data[nameof(PropertyName)]!;
    private set => Data[nameof(PropertyName)] = value;
  }

  public ValidationFailure Failure => new(PropertyName, ErrorMessage, Id)
  {
    ErrorCode = "AggregateNotFound"
  };

  private static string BuildMessage(Type type, string id, string propertyName)
  {
    StringBuilder message = new();

    message.AppendLine(ErrorMessage);
    message.Append("Type: ").AppendLine(type.GetName());
    message.Append("Id: ").AppendLine(id);
    message.Append("PropertyName: ").AppendLine(propertyName);

    return message.ToString();
  }
}

public class AggregateNotFoundException<T> : AggregateNotFoundException where T : AggregateRoot
{
  public AggregateNotFoundException(string id, string propertyName)
    : base(typeof(T), id, propertyName)
  {
  }
}
