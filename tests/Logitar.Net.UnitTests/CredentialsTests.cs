namespace Logitar.Net;

[Trait(Traits.Category, Categories.Unit)]
public class CredentialsTests
{
  [Theory(DisplayName = "ctor: it should construct the correct credentials.")]
  [InlineData("admin", "P@s$W0rD")]
  public void ctor_it_should_construct_the_correct_credentials(string identifier, string secret)
  {
    Credentials credentials = new(identifier, secret);
    Assert.Equal(identifier, credentials.Identifier);
    Assert.Equal(secret, credentials.Secret);
  }

  [Theory(DisplayName = "Parse: it should return null when the value is null or white space.")]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("    ")]
  public void Parse_it_should_return_null_when_the_value_is_null_or_white_space(string? credentials)
  {
    Assert.Null(Credentials.Parse(credentials));
  }

  [Theory(DisplayName = "Parse: it should return the correct credentials.")]
  [InlineData("admin")]
  [InlineData("admin", "P@s$W0rD")]
  public void Parse_it_should_return_the_correct_credentials(string identifier, string? secret = null)
  {
    StringBuilder s = new(identifier);
    if (secret != null)
    {
      s.Append(':').Append(secret);
    }

    Credentials? credentials = Credentials.Parse(s.ToString());
    Assert.NotNull(credentials);
    Assert.Equal(identifier, credentials.Identifier);
    Assert.Equal(secret ?? string.Empty, credentials.Secret);
  }
}
