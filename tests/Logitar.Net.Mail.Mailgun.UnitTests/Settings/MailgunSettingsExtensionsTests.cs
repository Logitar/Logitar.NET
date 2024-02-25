using Bogus;
using Logitar.Net.Http;

namespace Logitar.Net.Mail.Mailgun.Settings;

[Trait(Traits.Category, Categories.Unit)]
public class MailgunSettingsExtensionsTests
{
  private readonly Faker _faker = new();

  [Theory(DisplayName = "ToHttpApiSettings: authorization should be null when the API key is null.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("  ")]
  public void ToHttpApiSettings_authorization_should_be_null_when_the_Api_key_is_null_or_white_space(string? apiKey)
  {
    Assert.True(string.IsNullOrWhiteSpace(apiKey));

    MailgunSettings settings = new(apiKey, _faker.Internet.DomainName());
    IHttpApiSettings apiSettings = settings.ToHttpApiSettings();
    Assert.Null(apiSettings.Authorization);
  }

  [Fact(DisplayName = "ToHttpApiSettings: it should return the correct HTTP API settings.")]
  public void ToHttpApiSettings_it_should_return_the_correct_Http_Api_settings()
  {
    MailgunSettings settings = new(MailgunHelper.GenerateApiKey(), _faker.Internet.DomainName());
    IHttpApiSettings apiSettings = settings.ToHttpApiSettings();
    Assert.Equal(settings.BaseUri, apiSettings.BaseUri);

    Assert.NotNull(apiSettings.Authorization);
    Assert.Equal(Http.AuthenticationSchemes.Basic, apiSettings.Authorization.Scheme);

    Assert.NotNull(apiSettings.Authorization.Credentials);
    Credentials? credentials = Credentials.Parse(Encoding.ASCII.GetString(Convert.FromBase64String(apiSettings.Authorization.Credentials)));
    Assert.NotNull(credentials);
    Assert.Equal(settings.Username, credentials.Identifier);
    Assert.Equal(settings.Password, credentials.Secret);
  }
}
