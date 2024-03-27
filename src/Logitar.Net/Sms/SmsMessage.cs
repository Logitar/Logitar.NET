namespace Logitar.Net.Sms;

/// <summary>
/// Represents a text message.
/// </summary>
public record SmsMessage
{
  /// <summary>
  /// The maximum number of characters in a message text content.
  /// </summary>
  public const int MaximumLength = 1600;

  /// <summary>
  /// Gets the message sender as an E.164 formatted phone number.
  /// </summary>
  public string From { get; }
  /// <summary>
  /// Gets the message recipient as an E.164 formatted phone number.
  /// </summary>
  public string To { get; }

  /// <summary>
  /// Gets the text content of the message.
  /// </summary>
  public string Body { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="SmsMessage"/> class.
  /// </summary>
  /// <param name="from">The message sender as an E.164 formatted phone number.</param>
  /// <param name="to">The message recipient as an E.164 formatted phone number.</param>
  /// <param name="body">The text content of the message.</param>
  public SmsMessage(string from, string to, string body)
  {
    From = FormatToE164(from) ?? throw new ArgumentException($"The value '{from}' is not a valid phone number.", nameof(from));
    To = FormatToE164(to) ?? throw new ArgumentException($"The value '{to}' is not a valid phone number.", nameof(to));

    Body = body.CleanTrim() ?? throw new ArgumentException("The text content of the message is required.", nameof(body));
    if (body.Length > MaximumLength)
    {
      throw new ArgumentOutOfRangeException(nameof(body), $"The text content of the message must not exceed {MaximumLength} characters.");
    }
  }

  private static string? FormatToE164(string? phoneNumber)
  {
    char[] digits = phoneNumber?.Where(char.IsDigit).ToArray() ?? [];
    return digits.Length switch
    {
      10 => string.Concat("+1", new string(digits)), // NOTE(fpion): zone 1 is used in Canada and the United States.
      11 => string.Concat('+', new string(digits)), // NOTE(fpion): when the zone is already included in the number.
      _ => null,
    };
  }
}
