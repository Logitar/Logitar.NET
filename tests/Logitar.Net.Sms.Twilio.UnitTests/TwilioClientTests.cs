using Logitar.Net.Http;
using Logitar.Net.Sms.Twilio.Settings;

namespace Logitar.Net.Sms.Twilio;

[Trait(Traits.Category, Categories.Unit)]
public class TwilioClientTests : IDisposable
{
  private readonly string _accountSid = TwilioHelper.GenerateAccountSid();
  private readonly string _authenticationToken = TwilioHelper.GenerateAuthenticationToken();
  private readonly HttpClient _httpClient = new();

  public void Dispose()
  {
    _httpClient.Dispose();
    GC.SuppressFinalize(this);
  }

  [Fact(DisplayName = "ctor: it should construct a client with credentials.")]
  public void ctor_it_should_construct_a_client_with_credentials()
  {
    using TwilioClientMock client = new(_accountSid, _authenticationToken);
    Assert.True(client.DisposeClient);

    TwilioSettings settings = new(_accountSid, _authenticationToken);
    Assert.Equal(settings, client.Settings);

    AssertApplied(settings, client);
  }

  [Fact(DisplayName = "ctor: it should construct a client with the specified HttpClient and settings.")]
  public void ctor_it_should_construct_a_client_with_the_specified_HttpClient_and_settings()
  {
    TwilioSettings settings = new(_accountSid, _authenticationToken);
    TwilioClientMock client = new(_httpClient, settings);
    Assert.False(client.DisposeClient);

    AssertApplied(settings, client);
  }

  [Fact(DisplayName = "ctor: it should construct a client with the specified settings.")]
  public void ctor_it_should_construct_a_client_with_the_specified_settings()
  {
    TwilioSettings settings = new(_accountSid, _authenticationToken);
    using TwilioClientMock client = new(settings);
    Assert.True(client.DisposeClient);

    Assert.Equal(settings, client.Settings);

    AssertApplied(settings, client);
  }

  [Fact(DisplayName = "ctor: it should construct the default client.")]
  public void ctor_it_should_construct_the_default_client()
  {
    using TwilioClientMock client = new();
    Assert.True(client.DisposeClient);

    TwilioSettings settings = new();
    Assert.Equal(settings, client.Settings);

    AssertApplied(settings, client);
  }

  [Fact(DisplayName = "GetData: it should return the correct data.")]
  public void GetData_it_should_return_the_correct_data()
  {
    JsonApiResult result = new()
    {
      Version = new(1, 1),
      Status = new HttpStatus(HttpStatusCode.Unauthorized),
      ReasonPhrase = "Missing Authorization header.",
      JsonContent = @"{""code"":401,""message"":""Unauthorized""}"
    };
    result.Headers.Add(new HttpHeader("Content-Type", MediaTypeNames.Application.Json));
    result.TrailingHeaders.Add(new HttpHeader("WWW-Authenticate", Http.AuthenticationSchemes.Bearer));

    TwilioClientMock client = new();
    IDictionary<string, object?> data = client.GetData(result);
    Assert.Equal(result.Version, data["Version"]);
    Assert.Equal(result.Status, data["Status"]);
    Assert.Equal(result.ReasonPhrase, data["ReasonPhrase"]);
    Assert.Equal(result.JsonContent, data["JsonContent"]);
    Assert.Equal(result.Headers, data["Headers"]);
    Assert.Equal(result.TrailingHeaders, data["TrailingHeaders"]);
  }

  private static void AssertApplied(ITwilioSettings settings, TwilioClientMock client)
  {
    PropertyInfo? property = client.Client.GetType().GetProperty("Client", BindingFlags.NonPublic | BindingFlags.Instance);
    Assert.NotNull(property);

    HttpClient? httpClient = property.GetValue(client.Client) as HttpClient;
    Assert.NotNull(httpClient);

    Assert.Equal(settings.BaseUri, httpClient.BaseAddress);

    if (string.IsNullOrWhiteSpace(settings.AccountSid) || string.IsNullOrWhiteSpace(settings.AuthenticationToken))
    {
      Assert.Null(httpClient.DefaultRequestHeaders.Authorization);
    }
    else
    {
      Assert.NotNull(httpClient.DefaultRequestHeaders.Authorization);
      Assert.Equal(Http.AuthenticationSchemes.Basic, httpClient.DefaultRequestHeaders.Authorization.Scheme);

      Assert.NotNull(httpClient.DefaultRequestHeaders.Authorization.Parameter);
      Credentials? credentials = Credentials.Parse(Encoding.ASCII.GetString(Convert.FromBase64String(httpClient.DefaultRequestHeaders.Authorization.Parameter)));
      Assert.NotNull(credentials);
      Assert.Equal(settings.AccountSid, credentials.Identifier);
      Assert.Equal(settings.AuthenticationToken, credentials.Secret);
    }
  }
}
