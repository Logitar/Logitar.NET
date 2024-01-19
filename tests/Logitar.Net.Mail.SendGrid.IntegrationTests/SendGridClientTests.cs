using Logitar.Net.Http;
using Logitar.Net.Mail.SendGrid.Payloads;
using Microsoft.Extensions.Configuration;

namespace Logitar.Net.Mail.SendGrid;

[Trait(Traits.Category, Categories.Integration)]
public class SendGridClientTests : IDisposable
{
  private readonly CancellationToken _cancellationToken = default;

  private readonly MailAddress _from;
  private readonly MailAddress _to;
  private readonly SendGridClient _client;

  public SendGridClientTests()
  {
    IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
      .AddUserSecrets("0186de17-901c-4f62-b4ae-99c208289df8")
      .Build();

    EmailPayload from = configuration.GetSection("From").Get<EmailPayload>() ?? throw new InvalidOperationException("The configuration section 'From' is required.");
    _from = new(from.Address, from.DisplayName);

    EmailPayload to = configuration.GetSection("To").Get<EmailPayload>() ?? throw new InvalidOperationException("The configuration section 'To' is required.");
    _to = new(to.Address, to.DisplayName);

    string? apiKey = configuration.GetValue<string>("ApiKey") ?? throw new InvalidOperationException("The configuration 'ApiKey' is required.");
    _client = new(apiKey);
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
      .Replace("{name}", _to.DisplayName)
      .Replace("{token}", "eyJhbGciOiJIUzI1NiIsInR5cCI6InJlc2V0X3Bhc3N3b3JkK2p3dCJ9.eyJzdWIiOiI1NzJmZDBjNy0yMWUxLTRkNzktYTU3Yi0xYjFmODIwNDEwYmMiLCJpYXQiOjE1MTYyMzkwMjJ9.s7YJoA9HqabVDn_G9XBArKn9KjrHmj1EeFUZIuB6J9c");
    MailMessage message = new(_from, _to)
    {
      Subject = "Reset your password",
      Body = body,
      IsBodyHtml = true
    };

    SendMailResult result = await _client.SendAsync(message, _cancellationToken);
    Assert.True(result.Succeeded);
    Assert.Equal(new Version(1, 1), result.Data["Version"]);
    Assert.Equal(new HttpStatus(HttpStatusCode.Accepted), result.Data["Status"]);
    Assert.Equal(HttpStatusCode.Accepted.ToString(), result.Data["ReasonPhrase"]);
    Assert.True(result.Data.ContainsKey("Headers"));
    Assert.True(result.Data.ContainsKey("TrailingHeaders"));
    Assert.Null(result.Data["JsonContent"]);
  }
}
