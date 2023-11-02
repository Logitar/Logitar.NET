namespace Logitar;

/// <summary>
/// Provides extension methods for exceptions.
/// See <see cref="Exception"/> for more information.
/// </summary>
public static class ExceptionExtensions
{
  /// <summary>
  /// Formats an error code from the specified exception.
  /// </summary>
  /// <param name="exception">The exception to format from.</param>
  /// <returns>The formatted error code.</returns>
  public static string GetErrorCode(this Exception exception) => exception.GetType().Name.Remove(nameof(Exception));
}
