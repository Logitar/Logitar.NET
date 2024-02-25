namespace Logitar.Net.Mail.Mailgun;

internal record MailgunSendMessageResult
{
  [JsonPropertyName("id")]
  public string? Id { get; set; }

  [JsonPropertyName("message")]
  public string? Message { get; set; }
}
