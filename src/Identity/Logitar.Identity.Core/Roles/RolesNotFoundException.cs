using FluentValidation.Results;

namespace Logitar.Identity.Core.Roles;

public class RolesNotFoundException : Exception
{
  public const string ErrorMessage = "The specified roles could not be found.";

  public RolesNotFoundException(IEnumerable<string> ids, string propertyName)
    : base(BuildMessage(ids, propertyName))
  {
    Ids = ids;
    PropertyName = propertyName;
  }

  public IEnumerable<string> Ids
  {
    get => (IEnumerable<string>)Data[nameof(Ids)]!;
    set => Data[nameof(Ids)] = value;
  }
  public string PropertyName
  {
    get => (string)Data[nameof(PropertyName)]!;
    set => Data[nameof(PropertyName)] = value;
  }

  public ValidationFailure Failure => new(PropertyName, ErrorMessage)
  {
    AttemptedValue = string.Join(',', Ids),
    ErrorCode = "RolesNotFound"
  };

  private static string BuildMessage(IEnumerable<string> ids, string propertyName)
  {
    StringBuilder message = new();

    message.AppendLine(ErrorMessage);
    message.Append("PropertyName: ").AppendLine(propertyName);

    message.AppendLine("Ids:");
    foreach (string id in ids)
    {
      message.Append(" - ").AppendLine(id);
    }

    return message.ToString();
  }
}
