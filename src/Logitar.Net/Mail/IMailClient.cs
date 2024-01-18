namespace Logitar.Net.Mail;

/// <summary>
/// Defines methods to manage email messages.
/// </summary>
public interface IMailClient
{
  /// <summary>
  /// Sends the specified email message.
  /// </summary>
  /// <param name="message">The message to send.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The operation result.</returns>
  Task<SendMailResult> SendAsync(MailMessage message, CancellationToken cancellationToken = default);
}
