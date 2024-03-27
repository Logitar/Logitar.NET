namespace Logitar.Net.Sms.Twilio;

internal static class TwilioHelper
{
  public static string GenerateAccountSid()
  {
    return string.Join("AC", Guid.NewGuid().ToString("N"));
  }

  public static string GenerateAuthenticationToken()
  {
    byte[] bytes = RandomNumberGenerator.GetBytes(16);
    return new Guid(bytes).ToString("N");
  }
}
