using Logitar.Net.Http;

namespace Logitar.Net.Mail.SendGrid.Settings;

[Trait(Traits.Category, Categories.Unit)]
public class SendGridSettingsExtensionsTests
{
  [Theory(DisplayName = "ToHttpApiSettings: authorization should be null when the API key is null.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void ToHttpApiSettings_authorization_should_be_null_when_the_Api_key_is_null_or_white_space(string? apiKey)
  {
    Assert.True(string.IsNullOrWhiteSpace(apiKey));

    SendGridSettings settings = new(apiKey);
    IHttpApiSettings apiSettings = settings.ToHttpApiSettings();
    Assert.Null(apiSettings.Authorization);
  }

  [Fact(DisplayName = "ToHttpApiSettings: it should return the correct HTTP API settings.")]
  public void ToHttpApiSettings_it_should_return_the_correct_Http_Api_settings()
  {
    SendGridSettings settings = new(IntegrationTestHelper.GenerateApiKey());
    IHttpApiSettings apiSettings = settings.ToHttpApiSettings();
    Assert.Equal(settings.BaseUri, apiSettings.BaseUri);

    Assert.NotNull(apiSettings.Authorization);
    Assert.Equal(Http.AuthenticationSchemes.Bearer, apiSettings.Authorization.Scheme);
    Assert.Equal(settings.ApiKey, apiSettings.Authorization.Credentials);
  }
}
