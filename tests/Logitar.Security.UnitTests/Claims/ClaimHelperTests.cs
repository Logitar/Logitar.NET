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

    Claim claim = ClaimHelper.Create(name, moment);
    Assert.Equal(name, claim.Type);
    Assert.Equal(value, claim.Value);
    Assert.Equal(ClaimValueTypes.Integer64, claim.ValueType);
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

  private static long ToUnixTimeSeconds(DateTime moment) => new DateTimeOffset(moment).ToUnixTimeSeconds();
}
