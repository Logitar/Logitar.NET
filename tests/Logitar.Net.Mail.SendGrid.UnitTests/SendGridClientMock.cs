using Logitar.Net.Http;
using Logitar.Net.Mail.SendGrid.Settings;

namespace Logitar.Net.Mail.SendGrid;

internal class SendGridClientMock : SendGridClient
{
  public new JsonApiClient Client => base.Client;
  public new bool DisposeClient => base.DisposeClient;
  public new ISendGridSettings Settings => base.Settings;

  public SendGridClientMock() : base()
  {
  }

  public SendGridClientMock(string apiKey) : base(apiKey)
  {
  }

  public SendGridClientMock(ISendGridSettings settings) : base(settings)
  {
  }

  public SendGridClientMock(HttpClient client, ISendGridSettings settings) : base(client, settings)
  {
  }

  public new IDictionary<string, object?> GetData(JsonApiResult result)
  {
    return base.GetData(result);
  }
}
