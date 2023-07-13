namespace Logitar.Identity.Core.ApiKeys;

/// <summary>
/// The exception thrown when an <see cref="ApiKeyAggregate"/> expiration could be postponed.
/// </summary>
public class CannotPostponeApiKeyExpirationException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="CannotPostponeApiKeyExpirationException"/> class.
  /// </summary>
  /// <param name="apiKey">The API key that raised the exception.</param>
  /// <param name="expiresOn">The date and time that would postpone the expiration.</param>
  public CannotPostponeApiKeyExpirationException(ApiKeyAggregate apiKey, DateTime? expiresOn)
    : base(BuildMessage(apiKey, expiresOn))
  {
    Data[nameof(ApiKey)] = apiKey.ToString();
    Data[nameof(ExpiresOn)] = expiresOn;
  }

  /// <summary>
  /// Gets a string representation of the API key that raised the exception.
  /// </summary>
  public string ApiKey => (string)Data[nameof(ApiKey)]!;
  /// <summary>
  /// Gets the date and time that would postpone the expiration.
  /// </summary>
  public DateTime? ExpiresOn => (DateTime?)Data[nameof(ExpiresOn)];

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="apiKey">The API key that raised the exception.</param>
  /// <param name="expiresOn">The date and time that would postpone the expiration.</param>
  /// <returns>The exception message</returns>
  private static string BuildMessage(ApiKeyAggregate apiKey, DateTime? expiresOn)
  {
    StringBuilder message = new();

    message.AppendLine("The expiration of an API key cannot be postponed.");
    message.Append("ApiKey: ").Append(apiKey).AppendLine();
    message.Append("ExpiresOn: ").Append(expiresOn?.ToString() ?? "null").AppendLine();

    return message.ToString();
  }
}
