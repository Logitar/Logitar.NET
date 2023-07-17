namespace Logitar.Identity.Core.Sessions;

/// <summary>
/// The exception that is thrown when an action requiring an active session is attempted.
/// </summary>
public class SessionIsNotActiveException : Exception
{
  /// <summary>
  /// Initializes a new instance of the <see cref="SessionIsNotActiveException"/> class.
  /// </summary>
  /// <param name="session">The session that is not active.</param>
  public SessionIsNotActiveException(SessionAggregate session)
    : base($"The session '{session}' is not active.")
  {
    Data[nameof(Session)] = session.ToString();
  }

  /// <summary>
  /// Gets the session that is not active.
  /// </summary>
  public string Session => (string)Data[nameof(Session)]!;
}
