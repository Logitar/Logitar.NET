namespace Logitar.Net.Http;

public class JsonApiClient2 // TODO(fpion): interface
{
  private readonly HttpClient _client;

  public JsonApiClient2(HttpClient client)
  {
    _client = client;
  }

  public virtual async Task<JsonApiResponse<T>> SendAsync<T>(HttpMethod method, Uri uri, object? content, CancellationToken cancellationToken)
  {
    JsonContent? requestContent = content == null ? null : JsonContent.Create(content);

    using HttpRequestMessage request = BuildRequest(method, uri, requestContent);
    using HttpResponseMessage response = await _client.SendAsync(request, cancellationToken);

    return await BuildApiResponseAsync<T>(response, cancellationToken);
  }

  protected virtual HttpRequestMessage BuildRequest(HttpMethod method, Uri uri, HttpContent? content) => new HttpRequestBuilder()
    .SetMethod(method)
    .SetUrl(uri)
    .SetContent(content)
    // TODO(fpion): Authorization
    // TODO(fpion): Headers
    .BuildMessage();

  protected virtual async Task<JsonApiResponse<T>> BuildApiResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
  {
    JsonApiResponse<T> result = new(response);

    try
    {
      string? json = await response.Content.ReadAsStringAsync(cancellationToken);
      if (!string.IsNullOrEmpty(json))
      {
        result.ContentText = json;

        if (typeof(T) != typeof(Empty))
        {
          result.Value = JsonSerializer.Deserialize<T>(json); // TODO(fpion): serializer options
        }
      }
    }
    catch (Exception)
    {
    }

    return result;
  }
}
