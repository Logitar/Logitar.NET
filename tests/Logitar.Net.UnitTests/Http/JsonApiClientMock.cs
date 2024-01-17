namespace Logitar.Net.Http;

internal class JsonApiClientMock : JsonApiClient
{
  public new HttpClient Client => base.Client;
  public new bool DisposeClient => base.DisposeClient;
  public new bool ThrowOnFailure => base.ThrowOnFailure;

  public JsonApiClientMock() : base()
  {
  }

  public JsonApiClientMock(IHttpApiSettings settings) : base(settings)
  {
  }

  public JsonApiClientMock(HttpClient client) : base(client)
  {
  }

  public JsonApiClientMock(HttpClient client, IHttpApiSettings settings) : base(client, settings)
  {
  }

  public new async Task<JsonApiResult> BuildResultAsync(HttpResponseMessage response, CancellationToken cancellationToken)
  {
    return await base.BuildResultAsync(response, cancellationToken);
  }
}
