namespace Logitar.Net.Http;

/// <summary>
/// Provides extension methods for instance of the <see cref="HttpClient"/> class.
/// </summary>
public static class HttpClientExtensions
{
  /// <summary>
  /// Applies the specified settings to an instance of the <see cref="HttpClient"/> class.
  /// </summary>
  /// <param name="client">The HTTP client instance.</param>
  /// <param name="settings">The HTTP API settings.</param>
  public static void Apply(this HttpClient client, IHttpApiSettings settings)
  {
    client.BaseAddress = settings.BaseUri;

    foreach (HttpHeader header in settings.Headers)
    {
      client.DefaultRequestHeaders.Add(header.Name, header.Values);
    }

    IHttpAuthorization? authorization = settings.Authorization;
    if (authorization != null)
    {
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization.Scheme, authorization.Credentials);
    }
  }
}
