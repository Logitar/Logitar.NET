namespace Logitar.Net.Mail.Mailgun;

internal static class MailgunHelper
{
  public static string GenerateApiKey()
  {
    byte[] secret = RandomNumberGenerator.GetBytes(16);

    return string.Join('-', "key", new Guid(secret).ToString("N"));
  }
}
