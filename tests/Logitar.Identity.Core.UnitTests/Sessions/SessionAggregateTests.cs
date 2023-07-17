using FluentValidation;
using FluentValidation.Results;
using Logitar.EventSourcing;
using Logitar.Identity.Core.Sessions.Events;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users;

namespace Logitar.Identity.Core.Sessions;

[Trait(Traits.Category, Categories.Unit)]
public class SessionAggregateTests
{
  private readonly UniqueNameSettings _uniqueNameSettings = new()
  {
    AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"
  };

  private readonly UserAggregate _user;
  private readonly SessionAggregate _session;

  public SessionAggregateTests()
  {
    _user = new(_uniqueNameSettings, "admin");
    _session = new(_user, isPersistent: true);
  }

  [Theory(DisplayName = "It should be constructed correctly with_arguments.")]
  [InlineData("29ce7189-0557-463a-b0a3-9c6289d6772e")]
  public void It_should_be_constructed_correctly_with_arguments(string? id = null)
  {
    AggregateId? sessionId = id == null ? null : new(id);
    SessionAggregate session = new(_user, isPersistent: false, sessionId);
    Assert.Equal(_user.Id, session.UserId);
    Assert.False(session.IsPersistent);

    if (id == null)
    {
      Assert.NotEqual(default, session.Id);
    }
    else
    {
      Assert.Equal(sessionId, session.Id);
    }
  }

  [Theory(DisplayName = "It should be constructed correctly with identifier.")]
  [InlineData("123")]
  public void It_should_be_constructed_correctly_with_identifier(string id)
  {
    AggregateId sessionId = new(id);
    SessionAggregate session = new(sessionId);
    Assert.Equal(sessionId, session.Id);
  }

  [Fact(DisplayName = "It should be deleted correctly.")]
  public void It_should_be_deleted_correctly()
  {
    Assert.False(_session.IsDeleted);

    _session.Delete();
    Assert.True(_session.IsDeleted);
  }

  [Fact(DisplayName = "It should be signed out correctly.")]
  public void It_should_be_signed_out_correctly()
  {
    Assert.True(_session.IsActive);

    _session.SignOut();
    Assert.False(_session.IsActive);
    Assert.Contains(_session.Changes, e => e is SessionSignedOutEvent);
  }

  [Fact(DisplayName = "It should have a secret when it is persistent.")]
  public void It_should_have_a_secret_when_it_is_persistent()
  {
    SessionAggregate session = new(_user, isPersistent: true);
    Assert.True(session.IsPersistent);

    Assert.NotNull(session.Secret);
    Assert.Equal(32, session.Secret.Length);
  }

  [Fact(DisplayName = "It should not be signed out when it is not active.")]
  public void It_should_not_be_signed_out_when_it_is_not_active()
  {
    _session.SignOut();
    _session.ClearChanges();
    _session.SignOut();
    Assert.DoesNotContain(_session.Changes, e => e is SessionSignedOutEvent);
  }

  [Fact(DisplayName = "It should not remove custom attribute if it is not found.")]
  public void It_should_not_remove_custom_attribute_if_it_is_not_found()
  {
    _session.SetCustomAttribute("read_users", "true");

    _session.RemoveCustomAttribute("write_users");
    Assert.Contains(_session.CustomAttributes.Keys, key => key == "read_users");
    Assert.DoesNotContain(_session.CustomAttributes.Keys, key => key == "write_users");

    SessionModifiedEvent e = (SessionModifiedEvent)_session.Changes.Last(e => e is SessionModifiedEvent);
    Assert.DoesNotContain(e.CustomAttributes.Keys, key => key == "write_users");
  }
  [Fact(DisplayName = "It should remove custom attribute if it is found.")]
  public void It_should_remove_custom_attribute_if_it_is_found()
  {
    _session.SetCustomAttribute("read_users", "true");
    _session.SetCustomAttribute("write_users", "true");

    _session.RemoveCustomAttribute("   write_users ");
    Assert.Contains(_session.CustomAttributes.Keys, key => key == "read_users");
    Assert.DoesNotContain(_session.CustomAttributes.Keys, key => key == "write_users");

    SessionModifiedEvent e = (SessionModifiedEvent)_session.Changes.Last(e => e is SessionModifiedEvent);
    Assert.Null(e.CustomAttributes["write_users"]);
  }

  [Fact(DisplayName = "It should renew the session correctly.")]
  public void It_should_renew_the_session_correctly()
  {
    byte[]? oldSecret = _session.Secret;
    Assert.NotNull(oldSecret);

    _session.Renew(oldSecret);

    byte[]? newSecret = _session.Secret;
    Assert.NotNull(newSecret);

    Assert.Contains(_session.Changes, e => e is SessionRenewedEvent change && change.Secret.IsMatch(newSecret));

    Assert.Throws<InvalidCredentialsException>(() => _session.Renew(oldSecret));
  }

  [Theory(DisplayName = "It should set the correct custom attribute.")]
  [InlineData(" ProfileId ", "   76   ")]
  public void It_should_set_the_correct_custom_attribute(string key, string value)
  {
    _session.SetCustomAttribute(key, value);
    Assert.Equal(value.Trim(), _session.CustomAttributes[key.Trim()]);

    SessionModifiedEvent e = (SessionModifiedEvent)_session.Changes.Last(e => e is SessionModifiedEvent);
    Assert.Equal(value.Trim(), e.CustomAttributes[key.Trim()]);
  }
  [Fact(DisplayName = "It should set the correct custom attribute only if it is modified.")]
  public void It_should_set_the_correct_custom_attribute_only_if_it_is_modified()
  {
    _session.SetCustomAttribute("ProfileId", "51");
    SessionModifiedEvent e = (SessionModifiedEvent)_session.Changes.Last(e => e is SessionModifiedEvent);
    Assert.Equal("51", e.CustomAttributes["ProfileId"]);

    _session.ClearChanges();
    _session.SetCustomAttribute("ProfileId", "51");
    Assert.DoesNotContain(_session.Changes, e => e is SessionModifiedEvent);
  }

  [Fact(DisplayName = "It should throw InvalidCredentialsException when renewing a non persistent session.")]
  public void It_should_throw_InvalidCredentialsException_when_renewing_a_non_persistent_session()
  {
    SessionAggregate session = new(_user, isPersistent: false);
    Assert.Throws<InvalidCredentialsException>(() => session.Renew(Array.Empty<byte>()));
  }

  [Fact(DisplayName = "It should throw InvalidCredentialsException when renewing with invalid secret.")]
  public void It_should_throw_InvalidCredentialsException_when_renewing_with_invalid_secret()
  {
    Assert.NotNull(_session.Secret);
    Assert.Throws<InvalidCredentialsException>(() => _session.Renew(_session.Secret.Skip(1).ToArray()));
  }

  [Fact(DisplayName = "It should throw SessionIsNotActiveException when renewing an inactive session.")]
  public void It_should_throw_SessionIsNotActiveException_when_renewing_an_inactive_session()
  {
    _session.SignOut();
    Assert.NotNull(_session.Secret);
    var exception = Assert.Throws<SessionIsNotActiveException>(() => _session.Renew(_session.Secret));
    Assert.Equal(_session.ToString(), exception.Session);
  }

  [Fact(DisplayName = "It should throw ValidationException when custom attribute key is not valid.")]
  public void It_should_throw_ValidationException_when_custom_attribute_key_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _session.SetCustomAttribute(string.Empty, "true"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));
  }
  [Fact(DisplayName = "It should throw ValidationException when custom attribute value is not valid.")]
  public void It_should_throw_ValidationException_when_custom_attribute_value_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _session.SetCustomAttribute("write_users", string.Empty));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("Value", failure.PropertyName);
  }
}
