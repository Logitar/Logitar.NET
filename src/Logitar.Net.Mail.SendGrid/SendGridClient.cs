using Logitar.Net.Http;
using Logitar.Net.Mail.SendGrid.Payloads;
using Logitar.Net.Mail.SendGrid.Settings;

namespace Logitar.Net.Mail.SendGrid;

/// <summary>
/// Implements methods to manage email messages with SendGrid.
/// </summary>
public class SendGridClient : IDisposable, IMailClient
{
  /// <summary>
  /// Gets or sets the JSON API client used to send HTTP requests to the SendGrid API.
  /// </summary>
  protected virtual JsonApiClient Client { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not to dispose the API client when disposing this instance.
  /// </summary>
  protected virtual bool DisposeClient { get; set; }
  /// <summary>
  /// Gets or sets the settings of the SendGrid API.
  /// </summary>
  protected virtual ISendGridSettings Settings { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="SendGridClient"/> class.
  /// </summary>
  public SendGridClient() : this(new SendGridSettings())
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SendGridClient"/> class.
  /// </summary>
  /// <param name="apiKey">The API key used to authorize the SendGrid API calls.</param>
  public SendGridClient(string apiKey) : this(new SendGridSettings(apiKey))
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SendGridClient"/> class.
  /// </summary>
  /// <param name="settings">The SendGrid API settings.</param>
  public SendGridClient(ISendGridSettings settings) : this(new HttpClient(), settings)
  {
    DisposeClient = true;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="SendGridClient"/> class.
  /// </summary>
  /// <param name="client">An HTTP client intance.</param>
  /// <param name="settings">The SendGrid API settings.</param>
  public SendGridClient(HttpClient client, ISendGridSettings settings)
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
  /// Sends the specified email message.
  /// </summary>
  /// <param name="message">The message to send.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The operation result.</returns>
  public virtual async Task<SendMailResult> SendAsync(MailMessage message, CancellationToken cancellationToken)
  {
    SendMailPayload payload = new(message);

    EndPointSettings endPoint = Settings.SendMail;
    HttpRequestParameters parameters = new(endPoint.HttpMethod, endPoint.UriPath)
    {
      Content = JsonContent.Create(payload),
      ThrowOnFailure = false
    };

    JsonApiResult result = await Client.SendAsync(parameters, cancellationToken);
    return new SendMailResult(result.Status.IsSuccess, GetData(result));
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
