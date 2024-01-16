using Logitar.Net.Http.Settings;

namespace Logitar.Net.Http;

internal class FakeJsonApiClient : JsonApiClient
{
  public new HttpClient Client => base.Client;
  public new bool DisposeClient => base.DisposeClient;
  public new bool? ThrowOnFailure => base.ThrowOnFailure;
  public new JsonSerializerOptions? SerializerOptions => base.SerializerOptions;

  public FakeJsonApiClient() : base()
  {
  }

  public FakeJsonApiClient(IJsonApiSettings settings) : base(settings)
  {
  }

  public FakeJsonApiClient(HttpClient client) : base(client)
  {
  }

  public FakeJsonApiClient(HttpClient client, IJsonApiSettings settings) : base(client, settings)
  {
  }
}
