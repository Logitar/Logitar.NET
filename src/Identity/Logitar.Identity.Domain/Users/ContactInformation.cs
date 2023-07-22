namespace Logitar.Identity.Domain.Users;

public abstract record ContactInformation
{
  protected ContactInformation(bool isVerified = false)
  {
    IsVerified = isVerified;
  }

  public bool IsVerified { get; }
}
