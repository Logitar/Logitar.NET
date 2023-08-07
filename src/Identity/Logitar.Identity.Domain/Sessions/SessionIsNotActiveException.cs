namespace Logitar.Identity.Domain.Sessions;

public class SessionIsNotActiveException : Exception
{
  public SessionIsNotActiveException(SessionAggregate session)
    : base($"The session '{session}' is not active.")
  {
    Session = session.ToString();
  }

  public string Session
  {
    get => (string)Data[nameof(Session)]!;
    private set => Data[nameof(Session)] = value;
  }
}
