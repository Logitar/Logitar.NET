using FluentValidation;
using FluentValidation.Results;
using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys.Events;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Settings;

namespace Logitar.Identity.Core.ApiKeys;

[Trait(Traits.Category, Categories.Unit)]
public class ApiKeyAggregateTests
{
  private const string Title = "Default";

  private readonly Bogus.Faker _faker = new();
  private readonly UniqueNameSettings _uniqueNameSettings = new()
  {
    AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"
  };

  private readonly ApiKeyAggregate _apiKey;

  public ApiKeyAggregateTests()
  {
    _apiKey = new(Title);
  }

  [Fact(DisplayName = "AddRole: it should return false when role is present.")]
  public void AddRole_it_should_return_false_when_role_is_present()
  {
    RoleAggregate role = new(_uniqueNameSettings, "admin");
    _apiKey.AddRole(role);
    _apiKey.ClearChanges();

    Assert.False(_apiKey.AddRole(role));
    Assert.DoesNotContain(_apiKey.Changes, e => e is ApiKeyModifiedEvent);
  }
  [Fact(DisplayName = "AddRole: it should return true and add role when role is not present.")]
  public void AddRole_it_should_return_true_and_add_role_when_role_is_not_present()
  {
    Assert.Empty(_apiKey.Roles);

    RoleAggregate role = new(_uniqueNameSettings, "admin");
    Assert.True(_apiKey.AddRole(role));

    ApiKeyModifiedEvent e = (ApiKeyModifiedEvent)_apiKey.Changes.Last(e => e is ApiKeyModifiedEvent);
    Assert.True(e.Roles[role.Id.Value]);
  }

  [Fact(DisplayName = "Authenticate: it should throw ApiKeyIsExpiredException when it is expired.")]
  public void Authenticate_it_should_throw_ApiKeyIsExpiredException_when_it_is_expired()
  {
    _apiKey.ExpiresOn = DateTime.Now.AddDays(1);
    DateTime moment = DateTime.Now.AddDays(10);

    var exception = Assert.Throws<ApiKeyIsExpiredException>(() => _apiKey.Authenticate(_apiKey.Secret!, moment));
    Assert.Equal(_apiKey.ToString(), exception.ApiKey);
    Assert.Equal(moment, exception.Moment);
  }
  [Fact(DisplayName = "Authenticate: it should throw InvalidCredentialsException when secret is not valid.")]
  public void Authenticate_it_should_throw_InvalidCredentialsException_when_secret_is_not_valid()
  {
    Assert.Throws<InvalidCredentialsException>(() => _apiKey.Authenticate(_apiKey.Secret!.Skip(1).ToArray()));
  }

  [Fact(DisplayName = "Authentication should succeed when secret is valid and api key is not expired.")]
  public void Authentication_should_succeed_when_secret_is_valid_and_api_key_is_not_expired()
  {
    _apiKey.ExpiresOn = DateTime.Now.AddYears(1);

    DateTime moment = DateTime.Now.AddMonths(3);
    _apiKey.Authenticate(_apiKey.Secret!, moment);
    Assert.Equal(moment, _apiKey.AuthenticatedOn);
    Assert.Contains(_apiKey.Changes, e => e is ApiKeyAuthenticatedEvent);
  }

  [Fact(DisplayName = "Constructor should throw ValidationException when validation fails.")]
  public void Constructor_should_throw_ValidationException_when_validation_fails()
  {
    var exception = Assert.Throws<ValidationException>(() => new ApiKeyAggregate(string.Empty));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("NotEmptyValidator", failure.ErrorCode);
    Assert.Equal("Title", failure.PropertyName);
  }

  [Fact(DisplayName = "IsExpired: it should return false when expiration is null.")]
  public void IsExpired_it_should_return_false_when_expiration_is_null()
  {
    Assert.Null(_apiKey.ExpiresOn);
    Assert.False(_apiKey.IsExpired());
  }
  [Fact(DisplayName = "IsExpired: it should return false when it is not expired.")]
  public void IsExpired_it_should_return_false_when_it_is_not_expired()
  {
    DateTime expiresOn = DateTime.Now.AddDays(10);
    _apiKey.ExpiresOn = expiresOn;

    Assert.False(_apiKey.IsExpired(expiresOn.AddDays(-1)));
  }
  [Fact(DisplayName = "IsExpired: it should return true when it is expired.")]
  public void IsExpired_it_should_return_true_when_it_is_expired()
  {
    DateTime expiresOn = DateTime.Now.AddDays(10);
    _apiKey.ExpiresOn = expiresOn;

    Assert.True(_apiKey.IsExpired(expiresOn.AddDays(1)));
  }

  [Fact(DisplayName = "IsMatch: it should return false when the API key secret is not a match.")]
  public void IsMatch_it_should_return_false_when_the_API_key_secret_is_not_a_match()
  {
    Assert.False(_apiKey.IsMatch(_apiKey.Secret!.Skip(1).ToArray()));
  }
  [Fact(DisplayName = "IsMatch: it should return true when the API key secret is a match.")]
  public void IsMatch_it_should_return_true_when_the_API_key_secret_is_a_match()
  {
    Assert.True(_apiKey.IsMatch(_apiKey.Secret!));
  }

  [Theory(DisplayName = "It should be constructed correctly with_arguments.")]
  [InlineData(Title)]
  [InlineData(Title, "0adddb66-f1bf-49ba-85ed-b715998d083d")]
  [InlineData(" Default   ", "", "968")]
  public void It_should_be_constructed_correctly_with_arguments(string title, string? tenantId = null, string? id = null)
  {
    AggregateId? apiKeyId = id == null ? null : new(id);
    ApiKeyAggregate apiKey = new(title, tenantId, apiKeyId);
    Assert.Equal(title.Trim(), apiKey.Title);
    Assert.Equal(tenantId?.CleanTrim(), apiKey.TenantId);

    if (id == null)
    {
      Assert.NotEqual(default, apiKey.Id);
    }
    else
    {
      Assert.Equal(apiKeyId, apiKey.Id);
    }

    Assert.NotNull(apiKey.Secret);
    Assert.Equal(32, apiKey.Secret.Length);
  }

  [Theory(DisplayName = "It should be constructed correctly with identifier.")]
  [InlineData("123")]
  public void It_should_be_constructed_correctly_with_identifier(string id)
  {
    AggregateId apiKeyId = new(id);
    ApiKeyAggregate apiKey = new(apiKeyId);
    Assert.Equal(apiKeyId, apiKey.Id);
  }

  [Fact(DisplayName = "It should be deleted correctly.")]
  public void It_should_be_deleted_correctly()
  {
    Assert.False(_apiKey.IsDeleted);

    _apiKey.Delete();
    Assert.True(_apiKey.IsDeleted);
  }

  [Fact(DisplayName = "It should not remove custom attribute if it is not found.")]
  public void It_should_not_remove_custom_attribute_if_it_is_not_found()
  {
    _apiKey.SetCustomAttribute("read_users", "true");

    _apiKey.RemoveCustomAttribute("write_users");
    Assert.Contains(_apiKey.CustomAttributes.Keys, key => key == "read_users");
    Assert.DoesNotContain(_apiKey.CustomAttributes.Keys, key => key == "write_users");

    ApiKeyModifiedEvent e = (ApiKeyModifiedEvent)_apiKey.Changes.Last(e => e is ApiKeyModifiedEvent);
    Assert.DoesNotContain(e.CustomAttributes.Keys, key => key == "write_users");
  }
  [Fact(DisplayName = "It should remove custom attribute if it is found.")]
  public void It_should_remove_custom_attribute_if_it_is_found()
  {
    _apiKey.SetCustomAttribute("read_users", "true");
    _apiKey.SetCustomAttribute("write_users", "true");

    _apiKey.RemoveCustomAttribute("   write_users ");
    Assert.Contains(_apiKey.CustomAttributes.Keys, key => key == "read_users");
    Assert.DoesNotContain(_apiKey.CustomAttributes.Keys, key => key == "write_users");

    ApiKeyModifiedEvent e = (ApiKeyModifiedEvent)_apiKey.Changes.Last(e => e is ApiKeyModifiedEvent);
    Assert.Null(e.CustomAttributes["write_users"]);
  }

  [Fact(DisplayName = "It should return the correct string representation.")]
  public void It_should_return_the_correct_string_representation()
  {
    string expected = string.Format("{0} | {1} ({2})", _apiKey.Title, typeof(ApiKeyAggregate), _apiKey.Id);
    Assert.Equal(expected, _apiKey.ToString());
  }

  [Theory(DisplayName = "It should set the correct custom attribute.")]
  [InlineData(" ProfileId ", "   76   ")]
  public void It_should_set_the_correct_custom_attribute(string key, string value)
  {
    _apiKey.SetCustomAttribute(key, value);
    Assert.Equal(value.Trim(), _apiKey.CustomAttributes[key.Trim()]);

    ApiKeyModifiedEvent e = (ApiKeyModifiedEvent)_apiKey.Changes.Last(e => e is ApiKeyModifiedEvent);
    Assert.Equal(value.Trim(), e.CustomAttributes[key.Trim()]);
  }
  [Fact(DisplayName = "It should set the correct custom attribute only if it is modified.")]
  public void It_should_set_the_correct_custom_attribute_only_if_it_is_modified()
  {
    _apiKey.SetCustomAttribute("ProfileId", "51");
    ApiKeyModifiedEvent e = (ApiKeyModifiedEvent)_apiKey.Changes.Last(e => e is ApiKeyModifiedEvent);
    Assert.Equal("51", e.CustomAttributes["ProfileId"]);

    _apiKey.ClearChanges();
    _apiKey.SetCustomAttribute("ProfileId", "51");
    Assert.DoesNotContain(_apiKey.Changes, e => e is ApiKeyModifiedEvent);
  }

  [Theory(DisplayName = "It should set the correct Description.")]
  [InlineData(null)]
  [InlineData("  This is a test API key.  ")]
  public void It_should_set_the_correct_Description(string? description)
  {
    _apiKey.Description = description;
    Assert.Equal(description?.CleanTrim(), _apiKey.Description);
  }
  [Fact(DisplayName = "It should set the correct Description only if it is modified.")]
  public void It_should_set_the_correct_Description_only_if_it_is_modified()
  {
    _apiKey.Description = "This is a test API key.";
    ApiKeyModifiedEvent e = (ApiKeyModifiedEvent)_apiKey.Changes.Last(e => e is ApiKeyModifiedEvent);
    Assert.True(e.Description.IsModified);

    _apiKey.ClearChanges();
    _apiKey.Description = "This is a test API key.";
    Assert.DoesNotContain(_apiKey.Changes, e => e is ApiKeyModifiedEvent);
  }

  [Fact(DisplayName = "It should set the correct expiration date.")]
  public void It_should_set_the_correct_expiration_date()
  {
    DateTime expiresOn = DateTime.Now.AddMonths(6);

    _apiKey.ExpiresOn = expiresOn;
    Assert.Equal(expiresOn, _apiKey.ExpiresOn);
  }
  [Fact(DisplayName = "It should set the correct expiration date only if it is modified.")]
  public void It_should_set_the_correct_expiration_date_only_if_it_is_modified()
  {
    DateTime expiresOn = DateTime.Now.AddMonths(6);

    _apiKey.ExpiresOn = expiresOn;
    ApiKeyModifiedEvent e = (ApiKeyModifiedEvent)_apiKey.Changes.Last(e => e is ApiKeyModifiedEvent);
    Assert.True(e.ExpiresOn.IsModified);

    _apiKey.ClearChanges();
    _apiKey.ExpiresOn = expiresOn;
    Assert.DoesNotContain(_apiKey.Changes, e => e is ApiKeyModifiedEvent);
  }

  [Theory(DisplayName = "It should set the correct Title.")]
  [InlineData(Title)]
  [InlineData("  Default  ")]
  public void It_should_set_the_correct_Title(string title)
  {
    _apiKey.Title = title;
    Assert.Equal(title.Trim(), _apiKey.Title);
  }
  [Fact(DisplayName = "It should set the correct Title only if it is modified.")]
  public void It_should_set_the_correct_Title_only_if_it_is_modified()
  {
    _apiKey.Title = string.Concat(Title, '*');
    ApiKeyModifiedEvent e = (ApiKeyModifiedEvent)_apiKey.Changes.Last(e => e is ApiKeyModifiedEvent);
    Assert.True(e.Title.IsModified);

    _apiKey.ClearChanges();
    _apiKey.Title = string.Concat(Title, '*');
    Assert.DoesNotContain(_apiKey.Changes, e => e is ApiKeyModifiedEvent);
  }

  [Fact(DisplayName = "It should throw CannotPostponeApiKeyExpirationException when expiration is postponed.")]
  public void It_should_throw_CannotPostponeApiKeyExpirationException_when_expiration_is_postponed()
  {
    _apiKey.ExpiresOn = DateTime.Now.AddMonths(6);

    DateTime expiresOn = DateTime.Now.AddMonths(12);
    var exception = Assert.Throws<CannotPostponeApiKeyExpirationException>(() => _apiKey.ExpiresOn = expiresOn);
    Assert.Equal(_apiKey.ToString(), exception.ApiKey);
    Assert.Equal(expiresOn, exception.ExpiresOn);
  }
  [Fact(DisplayName = "It should throw CannotPostponeApiKeyExpirationException when expiration is postponed (null).")]
  public void It_should_throw_CannotPostponeApiKeyExpirationException_when_expiration_is_postponed_null()
  {
    _apiKey.ExpiresOn = DateTime.Now.AddMonths(6);

    var exception = Assert.Throws<CannotPostponeApiKeyExpirationException>(() => _apiKey.ExpiresOn = null);
    Assert.Equal(_apiKey.ToString(), exception.ApiKey);
    Assert.Null(exception.ExpiresOn);
  }

  [Fact(DisplayName = "It should throw ValidationException when custom attribute key is not valid.")]
  public void It_should_throw_ValidationException_when_custom_attribute_key_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _apiKey.SetCustomAttribute(string.Empty, "true"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));
  }
  [Fact(DisplayName = "It should throw ValidationException when custom attribute value is not valid.")]
  public void It_should_throw_ValidationException_when_custom_attribute_value_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _apiKey.SetCustomAttribute("write_users", string.Empty));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("Value", failure.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when expiration is not valid.")]
  public void It_should_throw_ValidationException_when_expiration_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _apiKey.ExpiresOn = DateTime.Now);
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("FutureValidator", failure.ErrorCode);
    Assert.Equal("ExpiresOn", failure.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when title is not valid.")]
  public void It_should_throw_ValidationException_when_title_is_not_valid()
  {
    string value = _faker.Random.String(length: 300, minChar: 'a', maxChar: 'z');
    var exception = Assert.Throws<ValidationException>(() => _apiKey.Title = value);
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("MaximumLengthValidator", failure.ErrorCode);
    Assert.Equal("Title", failure.PropertyName);
  }

  [Fact(DisplayName = "RemoveRole: it should return false when role is not present.")]
  public void RemoveRole_it_should_return_false_when_role_is_not_present()
  {
    RoleAggregate role = new(_uniqueNameSettings, "admin");
    Assert.False(_apiKey.RemoveRole(role));
    Assert.DoesNotContain(_apiKey.Changes, e => e is ApiKeyModifiedEvent);
  }
  [Fact(DisplayName = "RemoveRole: it should return true and remove role when role is present.")]
  public void RemoveRole_it_should_return_true_and_remove_role_when_role_is_present()
  {
    RoleAggregate role = new(_uniqueNameSettings, "admin");
    _apiKey.AddRole(role);

    Assert.True(_apiKey.RemoveRole(role));
    ApiKeyModifiedEvent e = (ApiKeyModifiedEvent)_apiKey.Changes.Last(e => e is ApiKeyModifiedEvent);
    Assert.False(e.Roles[role.Id.Value]);
  }
}
