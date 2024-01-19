namespace Logitar.Net.Http;

[Trait(Traits.Category, Categories.Unit)]
public class HttpAuthorizationTests
{
  [Theory(DisplayName = "Basic: it should construct the correct authorization.")]
  [InlineData("admin", "P@s$W0rD")]
  public void Basic_it_should_construct_the_correct_authorization(string identifier, string secret)
  {
    string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{identifier}:{secret}"));

    HttpAuthorization authorization;

    authorization = HttpAuthorization.Basic(identifier, secret);
    Assert.Equal(AuthenticationSchemes.Basic, authorization.Scheme);
    Assert.Equal(credentials, authorization.Credentials);

    authorization = HttpAuthorization.Basic(new Credentials(identifier, secret));
    Assert.Equal(AuthenticationSchemes.Basic, authorization.Scheme);
    Assert.Equal(credentials, authorization.Credentials);

    authorization = HttpAuthorization.Basic(credentials);
    Assert.Equal(AuthenticationSchemes.Basic, authorization.Scheme);
    Assert.Equal(credentials, authorization.Credentials);
  }

  [Theory(DisplayName = "Bearer: it should construct the correct authorization.")]
  [InlineData("1b45024e-32ea-45de-ad5f-53ec1a66f3f8")]
  public void Bearer_it_should_construct_the_correct_authorization(string credentials)
  {
    HttpAuthorization authorization = HttpAuthorization.Bearer(credentials);
    Assert.Equal(AuthenticationSchemes.Bearer, authorization.Scheme);
    Assert.Equal(credentials, authorization.Credentials);
  }
}
