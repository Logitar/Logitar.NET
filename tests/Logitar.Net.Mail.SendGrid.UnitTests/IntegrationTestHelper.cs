namespace Logitar.Net.Mail.SendGrid;

internal static class IntegrationTestHelper
{
  public static string GenerateApiKey()
  {
    Guid id = Guid.NewGuid();
    byte[] secret = RandomNumberGenerator.GetBytes(32);

    return string.Join('.', "SG", Convert.ToBase64String(id.ToByteArray()).ToUriSafeBase64(), Convert.ToBase64String(secret).ToUriSafeBase64());
  }
}
