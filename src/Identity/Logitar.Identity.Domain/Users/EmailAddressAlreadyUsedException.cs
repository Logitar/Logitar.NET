using FluentValidation.Results;

namespace Logitar.Identity.Domain.Users;

public class EmailAddressAlreadyUsedException : Exception
{
  public const string ErrorMessage = "The specified email address is already used.";

  public EmailAddressAlreadyUsedException(string? tenantId, IEmailAddress email, string propertyName)
    : base(BuildMessage(tenantId, email, propertyName))
  {
    TenantId = tenantId;
    EmailAddress = email.Address;
    PropertyName = propertyName;
  }

  public string? TenantId
  {
    get => (string?)Data[nameof(TenantId)];
    private set => Data[nameof(TenantId)] = value;
  }
  public string EmailAddress
  {
    get => (string)Data[nameof(EmailAddress)]!;
    private set => Data[nameof(EmailAddress)] = value;
  }
  public string PropertyName
  {
    get => (string)Data[nameof(PropertyName)]!;
    private set => Data[nameof(PropertyName)] = value;
  }

  public ValidationFailure Failure => new(PropertyName, ErrorMessage, EmailAddress)
  {
    ErrorCode = "EmailAddressAlreadyUsed",
    CustomState = new { TenantId }
  };

  private static string BuildMessage(string? tenantId, IEmailAddress email, string propertyName)
  {
    StringBuilder message = new();

    message.AppendLine(ErrorMessage);
    message.Append("TenantId: ").AppendLine(tenantId ?? "<null>");
    message.Append("EmailAddress: ").AppendLine(email.Address);
    message.Append("PropertyName: ").AppendLine(propertyName);

    return message.ToString();
  }
}
