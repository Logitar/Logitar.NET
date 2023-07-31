namespace Logitar.Security.Cryptography;

/// <summary>
/// TODO(fpion): documentation
/// </summary>
public abstract record Password
{
  public const char Separator = ':';

  public static Password Default => new Pbkdf2(string.Empty);

  public abstract string Encode();

  public abstract bool IsMatch(byte[] password);
  public abstract bool IsMatch(string password);
}
