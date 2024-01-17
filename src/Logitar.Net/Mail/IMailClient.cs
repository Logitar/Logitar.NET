namespace Logitar.Net.Mail;

/// <summary>
/// Defines methods to manage email messages.
/// </summary>
public interface IMailClient
{
  /// <summary>
  /// Sends the specified message.
  /// </summary>
  /// <param name="message">The message to send.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  Task SendAsync(MailMessage message, CancellationToken cancellationToken = default); // TODO(fpion): return type
}
