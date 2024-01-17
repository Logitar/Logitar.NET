using Logitar.Net.Mail.SendGrid.Payloads;
using Logitar.Net.Mail.SendGrid.Settings;

namespace Logitar.Net.Mail.SendGrid;

/// <summary>
/// Implements methods to manage email messages using the SendGrid provider.
/// </summary>
public class SendGridClient : IMailClient
{
  /// <summary>
  /// Gets the instance of a client to send HTTP requests.
  /// </summary>
  protected HttpClient HttpClient { get; }
  /// <summary>
  /// Gets the SendGrid API settings resolver.
  /// </summary>
  protected ISendGridSettingsResolver SettingsResolver { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="SendGridClient"/> class.
  /// </summary>
  /// <param name="httpClient">A client to send HTTP requests.</param>
  /// <param name="settingsResolver">The SendGrid API settings resolver.</param>
  public SendGridClient(HttpClient httpClient, ISendGridSettingsResolver settingsResolver)
  {
    HttpClient = httpClient;
    SettingsResolver = settingsResolver;
  }

  /// <summary>
  /// Sends the specified message.
  /// </summary>
  /// <param name="message">The message to send.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public virtual async Task SendAsync(MailMessage message, CancellationToken cancellationToken) // TODO(fpion): return type
  {
    SendMailPayload payload = BuildSendMailPayload(message);

    using HttpRequestMessage request = BuildSendMailRequest(payload);
    using HttpResponseMessage response = await HttpClient.SendAsync(request, cancellationToken);

    //HttpResponseDetail detail = await response.DetailAsync(cancellationToken); // TODO(fpion): implement
    try
    {
      response.EnsureSuccessStatusCode();
    }
    catch (Exception)
    {
      //throw new HttpFailureException(detail, innerException); // TODO(fpion): implement
    }

    // TODO(fpion): do something with detail
  }
  /// <summary>
  /// Builds an instance of the <see cref="SendMailPayload"/> class from the specified message.
  /// The following message properties are not supported yet: AlternateViews, Attachments, BodyEncoding, BodyTransferEncoding, DeliveryNotificationOptions, Headers, HeadersEncoding, Priority, SubjectEncoding.
  /// </summary>
  /// <param name="message">The message from which to build the payload.</param>
  /// <returns>The built payload.</returns>
  protected virtual SendMailPayload BuildSendMailPayload(MailMessage message)
  {
    return SendMailPayload.FromMailMessage(message);
  }
  /// <summary>
  /// Builds the HTTP request to send an email message.
  /// </summary>
  /// <param name="payload">The email message payload.</param>
  /// <returns>The built request.</returns>
  protected virtual HttpRequestMessage BuildSendMailRequest(SendMailPayload payload)
  {
    ISendGridSettings settings = SettingsResolver.Resolve();

    IEndPointSettings endPoint = settings.SendMail;
    UriBuilder uriBuilder = new(settings.BaseUri)
    {
      Path = endPoint.Path
    };
    HttpRequestMessage request = new(endPoint.HttpMethod, uriBuilder.Uri)
    {
      Content = JsonContent.Create(payload)
    };

    IAuthorizationSettings authorization = settings.Authorization;
    if (authorization.ApiKey != null)
    {
      request.Headers.Authorization = new AuthenticationHeaderValue(authorization.Scheme, authorization.ApiKey);
    }

    return request;
  }
}
