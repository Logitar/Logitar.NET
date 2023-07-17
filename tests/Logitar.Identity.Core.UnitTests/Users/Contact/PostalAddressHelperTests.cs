using System.Collections.Immutable;

namespace Logitar.Identity.Core.Users.Contact;

[Trait(Traits.Category, Categories.Unit)]
public class PostalAddressHelperTests
{
  private readonly PostalAddressHelper _helper = new();

  [Theory(DisplayName = "It should be in the supported list when it is supported.")]
  [InlineData("CA")]
  public void It_should_be_in_the_supported_list_when_it_is_supported(string country)
  {
    Assert.Contains(_helper.SupportedCountries, e => e == country);
  }

  [Theory(DisplayName = "It should not be in the supported list when it is not supported.")]
  [InlineData("QC")]
  public void It_should_not_be_in_the_supported_list_when_it_is_not_supported(string country)
  {
    Assert.DoesNotContain(_helper.SupportedCountries, e => e == country);
  }

  [Theory(DisplayName = "It should remove the country settings when setting null settings.")]
  [InlineData("CA")]
  public void It_should_remove_the_country_settings_when_setting_null_settings(string country)
  {
    Assert.True(_helper.IsSupported(country));

    _helper.SetCountry(country, settings: null);
    Assert.False(_helper.IsSupported(country));
  }

  [Theory(DisplayName = "It should return false when a country is not supported.")]
  [InlineData("QC")]
  public void It_should_return_false_when_a_country_is_not_supported(string country)
  {
    Assert.False(_helper.IsSupported(country));
  }

  [Theory(DisplayName = "It should return null when a country is not supported.")]
  [InlineData("QC")]
  public void It_should_return_null_when_a_country_is_not_supported(string country)
  {
    Assert.Null(_helper.GetCountry(country));
  }

  [Theory(DisplayName = "It should return the settings when a country is supported.")]
  [InlineData("CA")]
  public void It_should_return_the_settings_when_a_country_is_supported(string country)
  {
    Assert.NotNull(_helper.GetCountry(country));
  }

  [Theory(DisplayName = "It should return true when a country is supported.")]
  [InlineData("CA")]
  public void It_should_return_true_when_a_country_is_supported(string country)
  {
    Assert.True(_helper.IsSupported(country));
  }

  [Fact(DisplayName = "It should set the correct country settings.")]
  public void It_should_set_the_correct_country_settings()
  {
    string country = "US";
    CountrySettings settings = new()
    {
      PostalCode = "\\d{5}",
      Regions = ImmutableHashSet.Create(new[] { "AK" })
    };
    _helper.SetCountry(country, settings);
    Assert.Same(settings, _helper.GetCountry(country));
  }
}
