namespace Logitar.Identity.Domain.Users;

public class IncorrectUserPasswordException : InvalidCredentialsException
{
  public IncorrectUserPasswordException(UserAggregate user, string password)
    : base(BuildMessage(user, password))
  {
    User = user.ToString();
    Password = password;
  }

  public string User
  {
    get => (string)Data[nameof(User)]!;
    private set => Data[nameof(User)] = value;
  }
  public string Password
  {
    get => (string)Data[nameof(Password)]!;
    private set => Data[nameof(Password)] = value;
  }

  private static string BuildMessage(UserAggregate user, string password)
  {
    StringBuilder message = new();

    message.AppendLine("The specified password did not match the user.");
    message.Append("User: ").Append(user).AppendLine();
    message.Append("Password: ").AppendLine(password);

    return message.ToString();
  }
}
