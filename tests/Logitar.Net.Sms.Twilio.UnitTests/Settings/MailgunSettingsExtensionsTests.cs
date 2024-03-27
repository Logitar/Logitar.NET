using Logitar.Net.Http;

namespace Logitar.Net.Sms.Twilio.Settings;

[Trait(Traits.Category, Categories.Unit)]
public class TwilioSettingsExtensionsTests
{
  [Theory(DisplayName = "ToHttpApiSettings: authorization should be null when the account security identifier are null.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void ToHttpApiSettings_authorization_should_be_null_when_the_account_security_identifier_is_null_or_white_space(string? accountSid)
  {
    Assert.True(string.IsNullOrWhiteSpace(accountSid));

    TwilioSettings settings = new(accountSid, TwilioHelper.GenerateAuthenticationToken());
    IHttpApiSettings apiSettings = settings.ToHttpApiSettings();
    Assert.Null(apiSettings.Authorization);
  }

  [Theory(DisplayName = "ToHttpApiSettings: authorization should be null when the authentication token are null.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void ToHttpApiSettings_authorization_should_be_null_when_the_authentication_token_is_null_or_white_space(string? authenticationToken)
  {
    Assert.True(string.IsNullOrWhiteSpace(authenticationToken));

    TwilioSettings settings = new(TwilioHelper.GenerateAccountSid(), authenticationToken);
    IHttpApiSettings apiSettings = settings.ToHttpApiSettings();
    Assert.Null(apiSettings.Authorization);
  }

  [Fact(DisplayName = "ToHttpApiSettings: it should return the correct HTTP API settings.")]
  public void ToHttpApiSettings_it_should_return_the_correct_Http_Api_settings()
  {
    TwilioSettings settings = new(TwilioHelper.GenerateAccountSid(), TwilioHelper.GenerateAuthenticationToken());
    IHttpApiSettings apiSettings = settings.ToHttpApiSettings();
    Assert.Equal(settings.BaseUri, apiSettings.BaseUri);

    Assert.NotNull(apiSettings.Authorization);
    Assert.Equal(Http.AuthenticationSchemes.Basic, apiSettings.Authorization.Scheme);

    Assert.NotNull(apiSettings.Authorization.Credentials);
    Credentials? credentials = Credentials.Parse(Encoding.ASCII.GetString(Convert.FromBase64String(apiSettings.Authorization.Credentials)));
    Assert.NotNull(credentials);
    Assert.Equal(settings.AccountSid, credentials.Identifier);
    Assert.Equal(settings.AuthenticationToken, credentials.Secret);
  }
}
