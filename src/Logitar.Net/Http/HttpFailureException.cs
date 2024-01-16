namespace Logitar.Net.Http;

/// <summary>
/// The exception thrown when an HTTP response message did not indicate a success status code.
/// </summary>
public class HttpFailureException : Exception
{
  /// <summary>
  /// Gets or sets the HTTP response message information.
  /// </summary>
  public ApiResponse Response
  {
    get => (ApiResponse)Data[nameof(Response)]!;
    private set => Data[nameof(Response)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpFailureException"/> class.
  /// </summary>
  /// <param name="response">The HTTP response message information.</param>
  /// <param name="innerException">The inner exception.</param>
  public HttpFailureException(ApiResponse response, Exception? innerException = null)
    : base("The remote API did not return a success status code.", innerException)
  {
    Response = response;
  }
}
