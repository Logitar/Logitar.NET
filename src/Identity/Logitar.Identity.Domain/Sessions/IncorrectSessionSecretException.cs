namespace Logitar.Identity.Domain.Sessions;

public class IncorrectSessionSecretException : InvalidCredentialsException
{
  public IncorrectSessionSecretException(SessionAggregate session, string secret)
    : base(BuildMessage(session, secret))
  {
    Session = session.ToString();
    Secret = secret;
  }

  public string Session
  {
    get => (string)Data[nameof(Session)]!;
    private set => Data[nameof(Session)] = value;
  }
  public string Secret
  {
    get => (string)Data[nameof(Secret)]!;
    private set => Data[nameof(Secret)] = value;
  }

  private static string BuildMessage(SessionAggregate session, string secret)
  {
    StringBuilder message = new();

    message.AppendLine("The specified secret did not match the session.");
    message.Append("Session: ").Append(session).AppendLine();
    message.Append("Secret: ").AppendLine(secret);

    return message.ToString();
  }
}
