using FluentValidation;
using Logitar.Net.Http;
using Logitar.Net.Mail.Mailgun.Settings;
using Microsoft.Extensions.Configuration;

namespace Logitar.Net.Mail.Mailgun;

[Trait(Traits.Category, Categories.Integration)]
public class MailgunClientTests : IDisposable
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly MailgunTestSettings _settings;
  private readonly MailgunClient _client;

  public MailgunClientTests()
  {
    IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
      .AddUserSecrets("e30e27d5-0d3b-47b4-8e2d-c819041f66cf")
      .Build();

    _settings = configuration.Get<MailgunTestSettings>() ?? new();
    new MailgunTestValidator().ValidateAndThrow(_settings);

    _client = new(_settings.ApiKey, _settings.DomainName);
  }

  public void Dispose()
  {
    _client.Dispose();
    GC.SuppressFinalize(this);
  }

  [Fact(DisplayName = "SendAsync: it should send an email message.")]
  public async Task SendAsync_it_should_send_an_email_message()
  {
    string body = (await File.ReadAllTextAsync("Templates/PasswordRecovery.html"))
      .Replace("{name}", _settings.To.DisplayName)
      .Replace("{token}", "eyJhbGciOiJIUzI1NiIsInR5cCI6InJlc2V0X3Bhc3N3b3JkK2p3dCJ9.eyJzdWIiOiI1NzJmZDBjNy0yMWUxLTRkNzktYTU3Yi0xYjFmODIwNDEwYmMiLCJpYXQiOjE1MTYyMzkwMjJ9.s7YJoA9HqabVDn_G9XBArKn9KjrHmj1EeFUZIuB6J9c");
    MailMessage message = new(_settings.From.ToMailAddress(), _settings.To.ToMailAddress())
    {
      Subject = "Reset your password",
      Body = body,
      IsBodyHtml = true
    };
    foreach (EmailSettings cc in _settings.CC)
    {
      message.CC.Add(cc.ToMailAddress());
    }
    foreach (EmailSettings bcc in _settings.Bcc)
    {
      message.Bcc.Add(bcc.ToMailAddress());
    }

    SendMailResult result = await _client.SendAsync(message, _cancellationToken);
    Assert.True(result.Succeeded);
    Assert.Equal(new Version(1, 1), result.Data["Version"]);
    Assert.Equal(new HttpStatus(HttpStatusCode.OK), result.Data["Status"]);
    Assert.Equal(HttpStatusCode.OK.ToString(), result.Data["ReasonPhrase"]);
    Assert.True(result.Data.ContainsKey("Headers"));
    Assert.True(result.Data.ContainsKey("TrailingHeaders"));

    Assert.True(result.Data.ContainsKey("JsonContent"));
    string? jsonContent = result.Data["JsonContent"] as string;
    Assert.NotNull(jsonContent);
    MailgunSendMessageResult? response = JsonSerializer.Deserialize<MailgunSendMessageResult>(jsonContent);
    Assert.NotNull(response);
    Assert.NotNull(response.Id);
    Assert.NotNull(response.Message);
  }
}
