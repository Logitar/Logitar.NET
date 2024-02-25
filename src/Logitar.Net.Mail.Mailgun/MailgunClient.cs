using Logitar.Net.Http;
using Logitar.Net.Mail.Mailgun.Settings;

namespace Logitar.Net.Mail.Mailgun;

/// <summary>
/// Implements methods to manage email messages with Mailgun.
/// </summary>
public class MailgunClient : IDisposable, IMailClient
{
  /// <summary>
  /// Gets or sets the JSON API client used to send HTTP requests to the Mailgun API.
  /// </summary>
  protected virtual JsonApiClient Client { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not to dispose the API client when disposing this instance.
  /// </summary>
  protected virtual bool DisposeClient { get; set; }
  /// <summary>
  /// Gets or sets the settings of the Mailgun API.
  /// </summary>
  protected virtual IMailgunSettings Settings { get; set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="MailgunClient"/> class.
  /// </summary>
  public MailgunClient() : this(new MailgunSettings())
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="MailgunClient"/> class.
  /// </summary>
  /// <param name="apiKey">The API key used to authorize the Mailgun API calls.</param>
  /// <param name="domainName">The name of the domain used to send messages from.</param>
  public MailgunClient(string apiKey, string domainName) : this(new MailgunSettings(apiKey, domainName))
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="MailgunClient"/> class.
  /// </summary>
  /// <param name="settings">The Mailgun API settings.</param>
  public MailgunClient(IMailgunSettings settings) : this(new HttpClient(), settings)
  {
    DisposeClient = true;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="MailgunClient"/> class.
  /// </summary>
  /// <param name="client">An HTTP client intance.</param>
  /// <param name="settings">The Mailgun API settings.</param>
  public MailgunClient(HttpClient client, IMailgunSettings settings)
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
    MailAddress from = message.Sender ?? message.From
      ?? throw new ArgumentException($"At least one of the following must be provided: {nameof(message.From)}, {nameof(message.Sender)}.", nameof(message));

    MultipartFormDataContent content = new()
    {
      { new StringContent(from.ToString()), "from" }, // TODO(fpion): will this work?
      { new StringContent(message.Subject), "subject" },
      { new StringContent(message.Body), message.IsBodyHtml ? "html" : "text" },
    };

    foreach (MailAddress recipient in message.To)
    {
      content.Add(new StringContent(recipient.ToString()), "to"); // TODO(fpion): will this work?
    }
    foreach (MailAddress recipient in message.CC)
    {
      content.Add(new StringContent(recipient.ToString()), "cc");
    }
    foreach (MailAddress recipient in message.Bcc)
    {
      content.Add(new StringContent(recipient.ToString()), "bcc");
    }

    EndPointSettings endPoint = Settings.SendMail;
    Uri requestUri = new(endPoint.Path.Replace("{DomainName}", Settings.DomainName), UriKind.RelativeOrAbsolute);
    HttpRequestParameters parameters = new(endPoint.HttpMethod, requestUri)
    {
      Content = content,
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
