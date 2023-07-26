using FluentValidation.Results;

namespace Logitar.Identity.Domain.ApiKeys;

public class CannotPostponeApiKeyExpirationException : Exception
{
  public const string ErrorMessage = "The API key expiration cannot be postponed.";

  public CannotPostponeApiKeyExpirationException(ApiKeyAggregate apiKey, DateTime? expiresOn, string propertyName)
    : base(BuildMessage(apiKey, expiresOn))
  {
    ApiKey = apiKey.ToString();
    ExpiresOn = expiresOn;
    PropertyName = propertyName;
  }

  public string ApiKey
  {
    get => (string)Data[nameof(ApiKey)]!;
    set => Data[nameof(ApiKey)] = value;
  }
  public DateTime? ExpiresOn
  {
    get => (DateTime?)Data[nameof(ExpiresOn)];
    set => Data[nameof(ExpiresOn)] = value;
  }
  public string PropertyName
  {
    get => (string)Data[nameof(PropertyName)]!;
    set => Data[nameof(PropertyName)] = value;
  }

  public ValidationFailure Failure => new(PropertyName, ErrorMessage, ExpiresOn)
  {
    ErrorCode = "CannotPostponeApiKeyExpiration"
  };

  private static string BuildMessage(ApiKeyAggregate apiKey, DateTime? expiresOn)
  {
    StringBuilder message = new();

    message.AppendLine(ErrorMessage);
    message.Append("ApiKey: ").AppendLine(apiKey.ToString());
    message.Append("ExpiresOn: ").Append(expiresOn).AppendLine();

    return message.ToString();
  }
}
