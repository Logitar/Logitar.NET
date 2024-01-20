namespace Logitar.Security.Claims;

[Trait(Traits.Category, Categories.Unit)]
public class ClaimHelperTests
{
  [Fact(DisplayName = "It should create the correct claim from a date and time.")]
  public void It_should_create_the_correct_claim_from_a_date_and_time()
  {
    string name = Rfc7519ClaimNames.ExpirationTime;
    DateTime moment = DateTime.Now;
    string value = ToUnixTimeSeconds(moment).ToString();

    Claim local = ClaimHelper.Create(name, moment);
    Assert.Equal(name, local.Type);
    Assert.Equal(value, local.Value);
    Assert.Equal(ClaimValueTypes.Integer64, local.ValueType);

    Claim utc = ClaimHelper.Create(name, moment.ToUniversalTime());
    Assert.Equal(local.Type, utc.Type);
    Assert.Equal(local.Value, utc.Value);
    Assert.Equal(local.ValueType, utc.ValueType);
  }

  [Fact(DisplayName = "It should create the correct claim from an unspecified kind date and time.")]
  public void It_should_create_the_correct_claim_from_an_unspecified_kind_date_and_time()
  {
    DateTime now = DateTime.Now;

    string name = Rfc7519ClaimNames.ExpirationTime;
    DateTime localMoment = new(now.Ticks, DateTimeKind.Unspecified);
    string value = ToUnixTimeSeconds(localMoment).ToString();

    Claim local = ClaimHelper.Create(name, localMoment, DateTimeKind.Local);
    Assert.Equal(name, local.Type);
    Assert.Equal(value, local.Value);
    Assert.Equal(ClaimValueTypes.Integer64, local.ValueType);

    DateTime utcMoment = new(now.ToUniversalTime().Ticks, DateTimeKind.Unspecified);
    Claim utc = ClaimHelper.Create(name, utcMoment, DateTimeKind.Utc);
    Assert.Equal(local.Type, utc.Type);
    Assert.Equal(local.Value, utc.Value);
    Assert.Equal(local.ValueType, utc.ValueType);
  }

  [Fact(DisplayName = "It should extract the correct date and time from a claim.")]
  public void It_should_extract_the_correct_date_and_time_from_a_claim()
  {
    DateTime moment = DateTime.Now;
    string value = ToUnixTimeSeconds(moment).ToString();
    Claim claim = new(Rfc7519ClaimNames.AuthenticationTime, value, ClaimValueTypes.Integer64);

    DateTime result = ClaimHelper.ExtractDateTime(claim);
    Assert.Equal(ToUnixTimeSeconds(moment), ToUnixTimeSeconds(result));
  }

  [Fact(DisplayName = "It should extract the same date and time from the claim it created.")]
  public void It_should_extract_the_same_date_and_time_from_the_claim_it_created()
  {
    DateTime moment = DateTime.Now;
    Claim claim = ClaimHelper.Create(Rfc7519ClaimNames.UpdatedAt, moment);

    DateTime result = ClaimHelper.ExtractDateTime(claim);
    Assert.Equal(ToUnixTimeSeconds(moment), ToUnixTimeSeconds(result));
  }

  [Fact(DisplayName = "It should throw ArgumentException when no DateTimeKind is specified.")]
  public void It_should_throw_ArgumentException_when_no_DateTimeKind_is_specified()
  {
    DateTime moment = new(DateTime.Now.Ticks, DateTimeKind.Unspecified);
    ArgumentException exception;

    exception = Assert.Throws<ArgumentException>(() => ClaimHelper.Create(Rfc7519ClaimNames.ExpirationTime, moment));
    Assert.StartsWith("The date and time kind must be specified.", exception.Message);
    Assert.Equal("moment", exception.ParamName);

    exception = Assert.Throws<ArgumentException>(() => ClaimHelper.Create(Rfc7519ClaimNames.ExpirationTime, moment, DateTimeKind.Unspecified));
    Assert.StartsWith("The date and time kind must be specified.", exception.Message);
    Assert.Equal("moment", exception.ParamName);
  }

  private static long ToUnixTimeSeconds(DateTime moment) => new DateTimeOffset(moment).ToUnixTimeSeconds();
}
