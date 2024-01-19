namespace Logitar.Net.Http;

/// <summary>
/// Implements a JSON HTTP API client.
/// </summary>
public class JsonApiClient : HttpApiClient<JsonApiResult>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="JsonApiClient"/> class.
  /// An instance of the <see cref="HttpClient"/> class will be created; this instance shall be disposed explicitly.
  /// </summary>
  public JsonApiClient() : base()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonApiClient"/> class.
  /// An instance of the <see cref="HttpClient"/> class will be created; this instance shall be disposed explicitly.
  /// </summary>
  /// <param name="settings">The HTTP API settings.</param>
  public JsonApiClient(IHttpApiSettings settings) : base(settings)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonApiClient"/> class.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  public JsonApiClient(HttpClient client) : base(client)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="JsonApiClient"/> class.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="settings">The HTTP API settings.</param>
  public JsonApiClient(HttpClient client, IHttpApiSettings settings) : base(client, settings)
  {
  }

  /// <summary>
  /// Builds an API result from the specified response.
  /// </summary>
  /// <param name="response">The HTTP response.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The built result.</returns>
  protected override async Task<JsonApiResult> BuildResultAsync(HttpResponseMessage response, CancellationToken cancellationToken)
  {
    return await JsonApiResult.FromResponseAsync(response, cancellationToken);
  }
}
