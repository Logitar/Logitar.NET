namespace Logitar.Net.Http.Settings;

/// <summary>
/// Defines the settings of an HTTP API client.
/// </summary>
public interface IHttpApiSettings
{
  /// <summary>
  /// Gets the authentication/authorization settings of the API.
  /// </summary>
  IAuthorizationSettings? Authorization { get; }

  /// <summary>
  /// Gets the base Uniform Resource Identifier (URI) of the API.
  /// </summary>
  Uri? BaseUri { get; }

  /// <summary>
  /// Gets the request headers.
  /// </summary>
  List<HttpHeader> Headers { get; }

  /// <summary>
  /// Gets a value indicating whether or not to throw an <see cref="HttpFailureException"/> when a HTTP response does not indicate success.
  /// </summary>
  bool? ThrowOnFailure { get; }
}
