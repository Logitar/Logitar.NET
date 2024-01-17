namespace Logitar.Net.Http;

public class HttpApiClient2 : ApiClient<HttpApiResponse> // TODO(fpion): interface
{
  public HttpApiClient2(HttpClient httpClient) : base(httpClient)
  {
  }

  protected override Task<HttpApiResponse> BuildResultAsync(HttpResponseMessage response, CancellationToken cancellationToken)
  {
    return Task.FromResult(new HttpApiResponse(response));
  }
}
