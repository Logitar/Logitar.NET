using Logitar.Net.Http;
using Logitar.Net.Sms.Twilio.Settings;

namespace Logitar.Net.Sms.Twilio;

/// <summary>
/// Implements methods to manage short-text messages (SMS) with Twilio.
/// </summary>
public class TwilioClient : IDisposable, ISmsClient
{
  /// <summary>
  /// Gets or sets the JSON API client used to send HTTP requests to the Twilio API.
  /// </summary>
  protected virtual JsonApiClient Client { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not to dispose the API client when disposing this instance.
  /// </summary>
  protected virtual bool DisposeClient { get; set; }
  /// <summary>
  /// Gets or sets the settings of the Twilio API.
  /// </summary>
  protected virtual ITwilioSettings Settings { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="TwilioClient"/> class.
  /// </summary>
  public TwilioClient() : this(new TwilioSettings())
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="TwilioClient"/> class.
  /// </summary>
  /// <param name="accountSid">The Twilio account Security IDentifier.</param>
  /// <param name="authenticationToken">The Twilio account authentication token.</param>
  public TwilioClient(string accountSid, string authenticationToken) : this(new TwilioSettings(accountSid, authenticationToken))
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="TwilioClient"/> class.
  /// </summary>
  /// <param name="settings">The Twilio API settings.</param>
  public TwilioClient(ITwilioSettings settings) : this(new HttpClient(), settings)
  {
    DisposeClient = true;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="TwilioClient"/> class.
  /// </summary>
  /// <param name="client">An HTTP client intance.</param>
  /// <param name="settings">The Twilio API settings.</param>
  public TwilioClient(HttpClient client, ITwilioSettings settings)
  {
    Client = new JsonApiClient(client, settings.ToHttpApiSettings());
    Settings = settings;
  }

  /// <summary>
  /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
  /// </summary>
  public virtual void Dispose()
  {
    if (DisposeClient)
    {
      Client.Dispose();
    }

    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Sends the specified text message.
  /// </summary>
  /// <param name="message">The message to send.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The operation result.</returns>
  public async Task<SendSmsResult> SendAsync(SmsMessage message, CancellationToken cancellationToken)
  {
    Dictionary<string, string> data = new()
    {
      ["From"] = message.From,
      ["To"] = message.To,
      ["Body"] = message.Body
    };
    FormUrlEncodedContent content = new(data);

    EndPointSettings endPoint = Settings.CreateMessage;
    Uri requestUri = new(endPoint.Path.Replace("{AccountSid}", Settings.AccountSid), UriKind.RelativeOrAbsolute);
    HttpRequestParameters parameters = new(endPoint.HttpMethod, requestUri)
    {
      Content = content,
      ThrowOnFailure = false
    };

    JsonApiResult result = await Client.SendAsync(parameters, cancellationToken);
    return new SendSmsResult(result.Status.IsSuccess, GetData(result));
  }

  /// <summary>
  /// Builds a data dictionary from the specified API result.
  /// </summary>
  /// <param name="result">The API result.</param>
  /// <returns>The data dictionary.</returns>
  protected virtual IDictionary<string, object?> GetData(JsonApiResult result) => new Dictionary<string, object?>
  {
    [nameof(result.Version)] = result.Version,
    [nameof(result.Status)] = result.Status,
    [nameof(result.ReasonPhrase)] = result.ReasonPhrase,
    [nameof(result.Headers)] = result.Headers,
    [nameof(result.TrailingHeaders)] = result.TrailingHeaders,
    [nameof(result.JsonContent)] = result.JsonContent
  };
}
