namespace Logitar.Net.Http;

public class HttpApiClientMock : HttpApiClient
{
  public new HttpClient Client => base.Client;
  public new bool DisposeClient => base.DisposeClient;
  public new bool ThrowOnFailure => base.ThrowOnFailure;

  public HttpApiClientMock() : base()
  {
  }

  public HttpApiClientMock(IHttpApiSettings settings) : base(settings)
  {
  }

  public HttpApiClientMock(HttpClient client) : base(client)
  {
  }

  public HttpApiClientMock(HttpClient client, IHttpApiSettings settings) : base(client, settings)
  {
  }

  public new async Task<HttpRequestMessage> BuildRequestAsync(HttpRequestParameters parameters, CancellationToken cancellationToken)
  {
    return await base.BuildRequestAsync(parameters, cancellationToken);
  }

  public async Task<HttpApiResult> BuildResultBaseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
  {
    return await base.BuildResultAsync(response, cancellationToken);
  }
  protected override async Task<HttpApiResult> BuildResultAsync(HttpResponseMessage response, CancellationToken cancellationToken)
  {
    return await JsonApiResult.FromResponseAsync(response, cancellationToken);
  }
}
