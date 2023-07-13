namespace Logitar.Identity.Core.ApiKeys;

/// <summary>
/// The exception that is thrown when an <see cref="ApiKeyAggregate"/> is expired.
/// </summary>
public class ApiKeyIsExpiredException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyIsExpiredException"/> class.
  /// </summary>
  /// <param name="apiKey">The API key that is expired.</param>
  /// <param name="moment">The moment that has been used to validate expiration.</param>
  public ApiKeyIsExpiredException(ApiKeyAggregate apiKey, DateTime? moment)
    : base(BuildMessage(apiKey, moment))
  {
    Data[nameof(ApiKey)] = apiKey.ToString();
    Data[nameof(Moment)] = moment ?? DateTime.Now;
  }

  /// <summary>
  /// Gets a string representation of the API key that is expired.
  /// </summary>
  public string ApiKey => (string)Data[nameof(ApiKey)]!;
  /// <summary>
  /// Gets the moment that has been used to validate expiration.
  /// </summary>
  public DateTime Moment => (DateTime)Data[nameof(Moment)]!;

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="apiKey">The API key that is expired.</param>
  /// <param name="moment">The moment that has been used to validate expiration.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(ApiKeyAggregate apiKey, DateTime? moment)
  {
    StringBuilder message = new();

    message.AppendLine("The specified API key is expired.");
    message.Append("ApiKey: ").Append(apiKey).AppendLine();
    message.Append("Moment: ").Append(moment ?? DateTime.Now).AppendLine();

    return message.ToString();
  }
}
