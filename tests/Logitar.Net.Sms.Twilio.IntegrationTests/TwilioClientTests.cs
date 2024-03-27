using FluentValidation;
using Logitar.Net.Http;
using Logitar.Net.Sms.Twilio.Settings;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text.Json;

namespace Logitar.Net.Sms.Twilio;

[Trait(Traits.Category, Categories.Integration)]
public class TwilioClientTests : IDisposable
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly TwilioTestSettings _settings;
  private readonly TwilioClient _client;

  public TwilioClientTests()
  {
    IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
      .AddUserSecrets("1a8a8348-d68f-4560-95fc-4282ee565f24")
      .Build();

    _settings = configuration.Get<TwilioTestSettings>() ?? new();
    new TwilioTestValidator().ValidateAndThrow(_settings);

    _client = new(_settings.AccountSid, _settings.AuthenticationToken);
  }

  public void Dispose()
  {
    _client.Dispose();
    GC.SuppressFinalize(this);
  }

  [Fact(DisplayName = "SendAsync: it should send a text message.")]
  public async Task SendAsync_it_should_send_a_text_message()
  {
    CreateMessageSettings settings = _settings.CreateMessage;

    SmsMessage message = new(settings.From, settings.To, settings.Body);

    SendSmsResult result = await _client.SendAsync(message, _cancellationToken);
    Assert.True(result.Succeeded);
    Assert.Equal(new Version(1, 1), result.Data["Version"]);
    Assert.Equal(new HttpStatus(HttpStatusCode.Created), result.Data["Status"]);
    Assert.Equal(HttpStatusCode.Created.ToString(), result.Data["ReasonPhrase"]);
    Assert.True(result.Data.ContainsKey("Headers"));
    Assert.True(result.Data.ContainsKey("TrailingHeaders"));

    Assert.True(result.Data.ContainsKey("JsonContent"));
    string? jsonContent = result.Data["JsonContent"] as string;
    Assert.NotNull(jsonContent);
    TwilioCreateMessageResult? response = JsonSerializer.Deserialize<TwilioCreateMessageResult>(jsonContent);
    Assert.NotNull(response);
    Assert.NotNull(response.Sid);
    Assert.Equal(_settings.AccountSid, response.AccountSid);
    Assert.Null(response.MessagingServiceSid);
    Assert.Equal(message.From, response.From);
    Assert.Equal(message.To, response.To);
    Assert.Equal(message.Body, response.Body);
    Assert.Equal(1.ToString(), response.SegmentNumber);
    Assert.Equal(0.ToString(), response.MediaCount);
    Assert.Null(response.ErrorCode);
    Assert.Null(response.ErrorMessage);
  }
}
