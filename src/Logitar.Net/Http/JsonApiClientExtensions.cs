namespace Logitar.Net.Http;

/// <summary>
/// Provides extension methods for instances of the <see cref="JsonApiClient"/> class.
/// </summary>
public static class JsonApiClientExtensions
{
  /// <summary>
  /// Executes a JSON DELETE request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<JsonApiResult> DeleteAsync(this JsonApiClient client, Uri uri, CancellationToken cancellationToken)
  {
    return await client.DeleteAsync(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON DELETE request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<JsonApiResult> DeleteAsync(this JsonApiClient client, Uri uri, JsonRequestOptions? options, CancellationToken cancellationToken)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Delete(uri);
    parameters.Apply(options);

    return await client.SendAsync(parameters, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON DELETE request.
  /// </summary>
  /// <typeparam name="T">The type to deserialize.</typeparam>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deserialized result.</returns>
  public static async Task<T?> DeleteAsync<T>(this JsonApiClient client, Uri uri, CancellationToken cancellationToken)
  {
    return await client.DeleteAsync<T>(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON DELETE request.
  /// </summary>
  /// <typeparam name="T">The type to deserialize.</typeparam>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deserialized result.</returns>
  public static async Task<T?> DeleteAsync<T>(this JsonApiClient client, Uri uri, JsonRequestOptions? options, CancellationToken cancellationToken)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Delete(uri);
    parameters.Apply(options);

    JsonApiResult result = await client.SendAsync(parameters, cancellationToken);
    return result.Deserialize<T>(options?.SerializerOptions);
  }

  /// <summary>
  /// Executes a JSON GET request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<JsonApiResult> GetAsync(this JsonApiClient client, Uri uri, CancellationToken cancellationToken = default)
  {
    return await client.GetAsync(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON GET request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<JsonApiResult> GetAsync(this JsonApiClient client, Uri uri, JsonRequestOptions? options, CancellationToken cancellationToken = default)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Get(uri);
    parameters.Apply(options);

    return await client.SendAsync(parameters, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON GET request.
  /// </summary>
  /// <typeparam name="T">The type to deserialize.</typeparam>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deserialized result.</returns>
  public static async Task<T?> GetAsync<T>(this JsonApiClient client, Uri uri, CancellationToken cancellationToken = default)
  {
    return await client.GetAsync<T>(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON GET request.
  /// </summary>
  /// <typeparam name="T">The type to deserialize.</typeparam>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deserialized result.</returns>
  public static async Task<T?> GetAsync<T>(this JsonApiClient client, Uri uri, JsonRequestOptions? options, CancellationToken cancellationToken = default)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Get(uri);
    parameters.Apply(options);

    JsonApiResult result = await client.SendAsync(parameters, cancellationToken);
    return result.Deserialize<T>(options?.SerializerOptions);
  }

  /// <summary>
  /// Executes a JSON PATCH request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<JsonApiResult> PatchAsync(this JsonApiClient client, Uri uri, CancellationToken cancellationToken = default)
  {
    return await client.PatchAsync(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON PATCH request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<JsonApiResult> PatchAsync(this JsonApiClient client, Uri uri, JsonRequestOptions? options, CancellationToken cancellationToken = default)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Patch(uri);
    parameters.Apply(options);

    return await client.SendAsync(parameters, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON PATCH request.
  /// </summary>
  /// <typeparam name="T">The type to deserialize.</typeparam>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deserialized result.</returns>
  public static async Task<T?> PatchAsync<T>(this JsonApiClient client, Uri uri, CancellationToken cancellationToken = default)
  {
    return await client.PatchAsync<T>(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON PATCH request.
  /// </summary>
  /// <typeparam name="T">The type to deserialize.</typeparam>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deserialized result.</returns>
  public static async Task<T?> PatchAsync<T>(this JsonApiClient client, Uri uri, JsonRequestOptions? options, CancellationToken cancellationToken = default)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Patch(uri);
    parameters.Apply(options);

    JsonApiResult result = await client.SendAsync(parameters, cancellationToken);
    return result.Deserialize<T>(options?.SerializerOptions);
  }

  /// <summary>
  /// Executes a JSON POST request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<JsonApiResult> PostAsync(this JsonApiClient client, Uri uri, CancellationToken cancellationToken = default)
  {
    return await client.PostAsync(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON POST request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<JsonApiResult> PostAsync(this JsonApiClient client, Uri uri, JsonRequestOptions? options, CancellationToken cancellationToken = default)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Post(uri);
    parameters.Apply(options);

    return await client.SendAsync(parameters, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON POST request.
  /// </summary>
  /// <typeparam name="T">The type to deserialize.</typeparam>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deserialized result.</returns>
  public static async Task<T?> PostAsync<T>(this JsonApiClient client, Uri uri, CancellationToken cancellationToken = default)
  {
    return await client.PostAsync<T>(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON POST request.
  /// </summary>
  /// <typeparam name="T">The type to deserialize.</typeparam>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deserialized result.</returns>
  public static async Task<T?> PostAsync<T>(this JsonApiClient client, Uri uri, JsonRequestOptions? options, CancellationToken cancellationToken = default)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Post(uri);
    parameters.Apply(options);

    JsonApiResult result = await client.SendAsync(parameters, cancellationToken);
    return result.Deserialize<T>(options?.SerializerOptions);
  }

  /// <summary>
  /// Executes a JSON PUT request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<JsonApiResult> PutAsync(this JsonApiClient client, Uri uri, CancellationToken cancellationToken = default)
  {
    return await client.PutAsync(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON PUT request.
  /// </summary>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The API result.</returns>
  public static async Task<JsonApiResult> PutAsync(this JsonApiClient client, Uri uri, JsonRequestOptions? options, CancellationToken cancellationToken = default)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Put(uri);
    parameters.Apply(options);

    return await client.SendAsync(parameters, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON PUT request.
  /// </summary>
  /// <typeparam name="T">The type to deserialize.</typeparam>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deserialized result.</returns>
  public static async Task<T?> PutAsync<T>(this JsonApiClient client, Uri uri, CancellationToken cancellationToken = default)
  {
    return await client.PutAsync<T>(uri, options: null, cancellationToken);
  }
  /// <summary>
  /// Executes a JSON PUT request.
  /// </summary>
  /// <typeparam name="T">The type to deserialize.</typeparam>
  /// <param name="client">The HTTP client.</param>
  /// <param name="uri">The request Uniform Resource Identifier (URI).</param>
  /// <param name="options">The request options.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The deserialized result.</returns>
  public static async Task<T?> PutAsync<T>(this JsonApiClient client, Uri uri, JsonRequestOptions? options, CancellationToken cancellationToken = default)
  {
    HttpRequestParameters parameters = HttpRequestParameters.Put(uri);
    parameters.Apply(options);

    JsonApiResult result = await client.SendAsync(parameters, cancellationToken);
    return result.Deserialize<T>(options?.SerializerOptions);
  }
}
