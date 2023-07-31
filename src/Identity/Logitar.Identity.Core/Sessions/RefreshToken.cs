using Logitar.EventSourcing;

namespace Logitar.Identity.Core.Sessions;

public record RefreshToken
{
  public const string Prefix = "RT";
  public const char Separator = '.';

  public RefreshToken(AggregateId id, byte[] secret)
  {
    Id = id;
    Secret = secret;
  }

  public AggregateId Id { get; }
  public byte[] Secret { get; }

  public static RefreshToken Parse(string s)
  {
    string[] values = s.Split(Separator);
    if (values.Length != 3 || values[0] != Prefix)
    {
      throw new ArgumentException($"The value '{s}' is not a valid refresh token.", nameof(s));
    }

    return new RefreshToken(new AggregateId(values[1]),
      Convert.FromBase64String(values[2].FromUriSafeBase64()));
  }
  public static bool TryParse(string s, out RefreshToken? result)
  {
    try
    {
      result = Parse(s);
      return true;
    }
    catch (Exception)
    {
      result = null;
      return false;
    }
  }

  public override string ToString() => string.Join(Separator, Prefix,
    Id.Value, Convert.ToBase64String(Secret).ToUriSafeBase64());
}
