namespace Logitar.Identity.Domain.Passwords;

public abstract record Password
{
  public abstract bool IsMatch(string password);
}
