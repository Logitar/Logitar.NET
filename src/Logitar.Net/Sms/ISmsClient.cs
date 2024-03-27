namespace Logitar.Net.Sms;

/// <summary>
/// Defines methods to manage short-text messages (SMS).
/// </summary>
public interface ISmsClient
{
  /// <summary>
  /// Sends the specified text message.
  /// </summary>
  /// <param name="message">The message to send.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The operation result.</returns>
  Task<SendSmsResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default);
}
