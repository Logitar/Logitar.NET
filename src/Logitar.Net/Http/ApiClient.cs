namespace Logitar.Net.Http;

public abstract class ApiClient<T>
{
  protected HttpClient HttpClient { get; }

  protected ApiClient(HttpClient httpClient)
  {
    HttpClient = httpClient;
  }

  public virtual async Task<T> SendAsync(HttpMethod method, Uri uri, HttpContent? content, CancellationToken cancellationToken)
  {
    using HttpRequestMessage request = BuildRequest(method, uri, content);
    using HttpResponseMessage response = await HttpClient.SendAsync(request, cancellationToken);

    return await BuildResultAsync(response, cancellationToken);
  }

  protected virtual HttpRequestMessage BuildRequest(HttpMethod method, Uri uri, HttpContent? content) => new HttpRequestBuilder()
    .SetMethod(method)
    .SetUrl(uri)
    .SetContent(content)
    // TODO(fpion): Authorization
    // TODO(fpion): Headers
    .BuildMessage();

  protected abstract Task<T> BuildResultAsync(HttpResponseMessage response, CancellationToken cancellationToken);
}
