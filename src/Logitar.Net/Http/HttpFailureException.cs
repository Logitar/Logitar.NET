namespace Logitar.Net.Http;

/// <summary>
/// The exception thrown when an HTTP response message did not indicate a success status code.
/// </summary>
public class HttpFailureException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="HttpFailureException"/> class.
  /// </summary>
  /// <param name="message">The exception message.</param>
  /// <param name="innerException">The inner exception.</param>
  public HttpFailureException(string message = "The remote API did not return a success status code.", Exception? innerException = null)
    : base(message, innerException)
  {
  }
}

/// <summary>
/// The exception thrown when an HTTP response message did not indicate a success status code.
/// </summary>
/// <typeparam name="T">The HTTP API result type.</typeparam>
public class HttpFailureException<T> : HttpFailureException
{
  /// <summary>
  /// Gets or sets the HTTP API result.
  /// </summary>
  public T Result
  {
    get => (T)Data[nameof(Result)]!;
    private set => Data[nameof(Result)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="HttpFailureException{T}"/> class.
  /// </summary>
  /// <param name="result">The HTTP API result.</param>
  /// <param name="innerException">The inner exception.</param>
  public HttpFailureException(T result, Exception? innerException = null)
  : base(innerException: innerException)
  {
    Result = result;
  }
}
