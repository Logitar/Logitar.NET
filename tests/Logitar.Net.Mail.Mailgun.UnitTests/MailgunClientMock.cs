using Logitar.Net.Http;
using Logitar.Net.Mail.Mailgun.Settings;

namespace Logitar.Net.Mail.Mailgun;

internal class MailgunClientMock : MailgunClient
{
  public new JsonApiClient Client => base.Client;
  public new bool DisposeClient => base.DisposeClient;
  public new IMailgunSettings Settings => base.Settings;

  public MailgunClientMock() : base()
  {
  }

  public MailgunClientMock(string apiKey, string domainName) : base(apiKey, domainName)
  {
  }

  public MailgunClientMock(IMailgunSettings settings) : base(settings)
  {
  }

  public MailgunClientMock(HttpClient client, IMailgunSettings settings) : base(client, settings)
  {
  }

  public new IDictionary<string, object?> GetData(JsonApiResult result)
  {
    return base.GetData(result);
  }
}
