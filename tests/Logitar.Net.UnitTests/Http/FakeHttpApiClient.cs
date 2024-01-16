using Logitar.Net.Http.Settings;

namespace Logitar.Net.Http;

internal class FakeHttpApiClient : HttpApiClient
{
  public new HttpClient Client => base.Client;
  public new bool DisposeClient => base.DisposeClient;
  public new bool? ThrowOnFailure => base.ThrowOnFailure;

  public FakeHttpApiClient() : base()
  {
  }

  public FakeHttpApiClient(IHttpApiSettings settings) : base(settings)
  {
  }

  public FakeHttpApiClient(HttpClient client) : base(client)
  {
  }

  public FakeHttpApiClient(HttpClient client, IHttpApiSettings settings) : base(client, settings)
  {
  }
}
