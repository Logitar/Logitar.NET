namespace Logitar.Identity.Core;

/// <summary>
/// The exception that is thrown when the specified credentials are not valid.
/// </summary>
public class InvalidCredentialsException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="InvalidCredentialsException"/> class.
  /// </summary>
  /// <param name="message">The exception message.</param>
  /// <param name="innerException">The inner exception.</param>
  public InvalidCredentialsException(string message, Exception? innerException = null)
    : base(message, innerException)
  {
  }
}
