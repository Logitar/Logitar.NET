using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Logitar.Security.Cryptography;

/// <summary>
/// TODO(fpion): documentation
/// </summary>
public record Pbkdf2Settings
{
  public KeyDerivationPrf Algorithm { get; set; } = KeyDerivationPrf.HMACSHA256;
  public int Iterations { get; set; } = 600000;
  public int SaltLength { get; set; } = 256 / 8;
  public int? HashLength { get; set; }
}
