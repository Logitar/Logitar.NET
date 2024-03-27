using System.Text.Json.Serialization;

namespace Logitar.Net.Sms.Twilio;

internal record TwilioCreateMessageResult
{
  [JsonPropertyName("sid")]
  public string? Sid { get; set; }

  [JsonPropertyName("account_sid")]
  public string? AccountSid { get; set; }

  [JsonPropertyName("messaging_service_sid")]
  public string? MessagingServiceSid { get; set; }
  [JsonPropertyName("from")]
  public string? From { get; set; }
  [JsonPropertyName("to")]
  public string? To { get; set; }

  [JsonPropertyName("body")]
  public string? Body { get; set; }
  [JsonPropertyName("num_segments")]
  public string? SegmentNumber { get; set; }
  [JsonPropertyName("num_media")]
  public string? MediaCount { get; set; }

  [JsonPropertyName("date_created")]
  public string? CreatedOn { get; set; }
  [JsonPropertyName("date_updated")]
  public string? UpdatedOn { get; set; }
  [JsonPropertyName("date_sent")]
  public string? SentOn { get; set; }
  [JsonPropertyName("status")]
  public string? Status { get; set; }

  [JsonPropertyName("price")]
  public decimal? Price { get; set; }
  [JsonPropertyName("price_unit")]
  public string? Currency { get; set; }

  [JsonPropertyName("direction")]
  public string? Direction { get; set; }
  [JsonPropertyName("api_version")]
  public string? ApiVersion { get; set; }
  [JsonPropertyName("uri")]
  public string? Uri { get; set; }

  [JsonPropertyName("error_code")]
  public string? ErrorCode { get; set; }
  [JsonPropertyName("error_message")]
  public string? ErrorMessage { get; set; }
}
