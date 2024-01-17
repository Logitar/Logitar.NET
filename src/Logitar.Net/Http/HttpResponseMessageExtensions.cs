namespace Logitar.Net.Http;

/// <summary>
/// Provides extension methods for instance of the <see cref="HttpResponseMessage"/> class.
/// </summary>
public static class HttpResponseMessageExtensions
{
  /// <summary>
  /// Throws an instance of the <see cref="HttpFailureException"/> class if the specified response did not indicate a success status code.
  /// </summary>
  /// <param name="response">The HTTP response.</param>
  /// <exception cref="HttpFailureException">The HTTP response did not indicate a success status code.</exception>
  public static void ThrowOnFailure(this HttpResponseMessage response)
  {
    try
    {
      response.EnsureSuccessStatusCode();
    }
    catch (Exception innerException)
    {
      throw new HttpFailureException(innerException: innerException);
    }
  }
  /// <summary>
  /// Throws an instance of the <see cref="HttpFailureException{T}"/> class if the specified response did not indicate a success status code.
  /// </summary>
  /// <typeparam name="T">The API result type.</typeparam>
  /// <param name="response">The HTTP response.</param>
  /// <param name="result">The API result.</param>
  /// <exception cref="HttpFailureException{T}">The HTTP response did not indicate a success status code.</exception>
  public static void ThrowOnFailure<T>(this HttpResponseMessage response, T result)
  {
    try
    {
      response.EnsureSuccessStatusCode();
    }
    catch (Exception innerException)
    {
      throw new HttpFailureException<T>(result, innerException);
    }
  }
}
