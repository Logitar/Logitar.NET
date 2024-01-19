namespace Logitar.Net.Http;

internal static class IntegrationTestHelper
{
  public static JsonSerializerOptions SerializerOptions { get; } = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

  public static string GenerateApiKey()
  {
    Guid id = Guid.NewGuid();
    byte[] secret = RandomNumberGenerator.GetBytes(32);

    return string.Join('.', "LT", Convert.ToBase64String(id.ToByteArray()).ToUriSafeBase64(), Convert.ToBase64String(secret).ToUriSafeBase64());
  }

  public static string GenerateSecret(int length = 32) => Convert.ToBase64String(RandomNumberGenerator.GetBytes(length));
}
