namespace Logitar.Identity.Domain.Users;

public abstract record Contact
{
  protected Contact(bool isVerified = false)
  {
    IsVerified = isVerified;
  }

  public bool IsVerified { get; }
}
