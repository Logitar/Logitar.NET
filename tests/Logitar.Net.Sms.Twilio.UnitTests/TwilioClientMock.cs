using Logitar.Net.Http;
using Logitar.Net.Sms.Twilio.Settings;

namespace Logitar.Net.Sms.Twilio;

internal class TwilioClientMock : TwilioClient
{
  public new JsonApiClient Client => base.Client;
  public new bool DisposeClient => base.DisposeClient;
  public new ITwilioSettings Settings => base.Settings;

  public TwilioClientMock() : base()
  {
  }

  public TwilioClientMock(string accountSid, string authenticationToken) : base(accountSid, authenticationToken)
  {
  }

  public TwilioClientMock(ITwilioSettings settings) : base(settings)
  {
  }

  public TwilioClientMock(HttpClient client, ITwilioSettings settings) : base(client, settings)
  {
  }

  public new IDictionary<string, object?> GetData(JsonApiResult result)
  {
    return base.GetData(result);
  }
}
