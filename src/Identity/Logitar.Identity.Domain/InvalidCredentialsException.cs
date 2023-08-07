namespace Logitar.Identity.Domain;

public class InvalidCredentialsException : Exception
{
  public InvalidCredentialsException(string message = "The specified credentials did not match.",
    Exception? innerException = null) : base(message, innerException)
  {
  }
}
