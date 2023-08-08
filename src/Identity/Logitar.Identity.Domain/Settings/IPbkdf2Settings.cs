using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Logitar.Identity.Domain.Settings;
public interface IPbkdf2Settings
{
  KeyDerivationPrf Algorithm { get; }
  int Iterations { get; }
  int SaltLength { get; }
  int? HashLength { get; }
}
