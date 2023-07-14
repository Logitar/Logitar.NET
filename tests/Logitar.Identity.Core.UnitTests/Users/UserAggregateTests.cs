﻿using FluentValidation;
using FluentValidation.Results;
using Logitar.EventSourcing;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Core.Users.Events;
using Logitar.Security.Cryptography;

namespace Logitar.Identity.Core.Users;

[Trait(Traits.Category, Categories.Unit)]
public class UserAggregateTests
{
  private const string UniqueName = "fpion";

  private readonly PasswordSettings _passwordSettings = new()
  {
    RequiredLength = 8,
    RequiredUniqueChars = 8,
    RequireNonAlphanumeric = true,
    RequireLowercase = true,
    RequireUppercase = true,
    RequireDigit = true
  };
  private readonly UniqueNameSettings _uniqueNameSettings = new()
  {
    AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"
  };

  private readonly UserAggregate _user;

  public UserAggregateTests()
  {
    _user = new(_uniqueNameSettings, UniqueName);
  }

  [Fact(DisplayName = "Constructor should throw ValidationException when validation fails.")]
  public void Constructor_should_throw_ValidationException_when_validation_fails()
  {
    var exception = Assert.Throws<ValidationException>(() => new UserAggregate(_uniqueNameSettings, "Test123!"));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("AllowedCharactersValidator", failure.ErrorCode);
    Assert.Equal("UniqueName", failure.PropertyName);
  }

  [Fact(DisplayName = "HasPassword: it should be false when the user has no password.")]
  public void HasPassword_it_should_be_false_when_the_user_has_no_password()
  {
    Assert.Null(GetPassword(_user));
    Assert.False(_user.HasPassword);
  }
  [Fact(DisplayName = "HasPassword: it should be true when the user has a password.")]
  public void HasPassword_it_should_be_true_when_the_user_has_a_password()
  {
    _user.ChangePassword(_passwordSettings, "P@s$W0rD");

    Assert.NotNull(GetPassword(_user));
    Assert.True(_user.HasPassword);
  }

  [Fact(DisplayName = "It should add a new role correctly.")]
  public void It_should_add_a_new_role_correctly()
  {
    RoleAggregate role = new(_uniqueNameSettings, "admin");
    _user.AddRole(role);
    Assert.True(_user.Roles.Contains(role.Id));
    Assert.Contains(_user.Changes, e => e is UserModifiedEvent change && change.Roles[role.Id.Value] == true);

    _user.ClearChanges();
    _user.AddRole(role);
    Assert.DoesNotContain(_user.Changes, e => e is UserModifiedEvent);
  }
  [Fact(DisplayName = "It should remove an existing role correctly.")]
  public void It_should_remove_an_existing_role_correctly()
  {
    RoleAggregate role = new(_uniqueNameSettings, "admin");
    _user.AddRole(role);

    _user.RemoveRole(role);
    Assert.False(_user.Roles.Contains(role.Id));
    Assert.Contains(_user.Changes, e => e is UserModifiedEvent change && change.Roles[role.Id.Value] == false);

    _user.ClearChanges();
    Assert.False(_user.Roles.Contains(role.Id));
    Assert.DoesNotContain(_user.Changes, e => e is UserModifiedEvent);
  }

  [Theory(DisplayName = "It should be constructed correctly with_arguments.")]
  [InlineData(UniqueName)]
  [InlineData(UniqueName, "0adddb66-f1bf-49ba-85ed-b715998d083d")]
  [InlineData(" admin   ", "", "968")]
  public void It_should_be_constructed_correctly_with_arguments(string uniqueName, string? tenantId = null, string? id = null)
  {
    AggregateId? roleId = id == null ? null : new(id);
    UserAggregate role = new(_uniqueNameSettings, uniqueName, tenantId, roleId);
    Assert.Equal(uniqueName.Trim(), role.UniqueName);
    Assert.Equal(tenantId?.CleanTrim(), role.TenantId);

    if (id == null)
    {
      Assert.NotEqual(default, role.Id);
    }
    else
    {
      Assert.Equal(roleId, role.Id);
    }
  }

  [Theory(DisplayName = "It should be constructed correctly with identifier.")]
  [InlineData("123")]
  public void It_should_be_constructed_correctly_with_identifier(string id)
  {
    AggregateId userId = new(id);
    UserAggregate user = new(userId);
    Assert.Equal(userId, user.Id);
  }

  [Fact(DisplayName = "It should be deleted correctly.")]
  public void It_should_be_deleted_correctly()
  {
    Assert.False(_user.IsDeleted);

    _user.Delete();
    Assert.True(_user.IsDeleted);
  }

  [Fact(DisplayName = "It should be disabled correctly.")]
  public void It_should_be_disabled_correctly()
  {
    _user.Disable();
    Assert.True(_user.IsDisabled);
    Assert.Contains(_user.Changes, e => e is UserStatusChangedEvent change && change.IsDisabled == true);

    _user.ClearChanges();
    _user.Disable();
    Assert.DoesNotContain(_user.Changes, e => e is UserStatusChangedEvent);
  }
  [Fact(DisplayName = "It should be disabled correctly.")]
  public void It_should_be_enabled_correctly()
  {
    _user.Disable();
    _user.ClearChanges();

    _user.Enable();
    Assert.False(_user.IsDisabled);
    Assert.Contains(_user.Changes, e => e is UserStatusChangedEvent @event && @event.IsDisabled == false);

    _user.ClearChanges();
    _user.Enable();
    Assert.DoesNotContain(_user.Changes, e => e is UserStatusChangedEvent);
  }

  [Fact(DisplayName = "It should change the password when it is valid and current is null.")]
  public void It_should_change_the_password_when_it_is_valid_and_current_is_null()
  {
    string passwordString = "Test123!";
    _user.ChangePassword(_passwordSettings, passwordString, current: null);
    Assert.Contains(_user.Changes, e => e is UserPasswordChangedEvent);

    Pbkdf2? password = GetPassword(_user);
    Assert.NotNull(password);
    Assert.True(password.IsMatch(passwordString));
  }

  [Fact(DisplayName = "It should change the password when it is valid and current is valid.")]
  public void It_should_change_the_password_when_it_is_valid_and_current_is_valid()
  {
    string current = "Test123!";
    _user.ChangePassword(_passwordSettings, current);

    string password = "P@s$W0rD";
    _user.ChangePassword(_passwordSettings, password, current);

    Pbkdf2? pbkdf2 = GetPassword(_user);
    Assert.NotNull(pbkdf2);
    Assert.True(pbkdf2.IsMatch(password));
  }

  [Fact(DisplayName = "It should not remove custom attribute if it is not found.")]
  public void It_should_not_remove_custom_attribute_if_it_is_not_found()
  {
    _user.SetCustomAttribute("read_users", "true");

    _user.RemoveCustomAttribute("write_users");
    Assert.Contains(_user.CustomAttributes.Keys, key => key == "read_users");
    Assert.DoesNotContain(_user.CustomAttributes.Keys, key => key == "write_users");

    UserModifiedEvent e = (UserModifiedEvent)_user.Changes.Last(e => e is UserModifiedEvent);
    Assert.DoesNotContain(e.CustomAttributes.Keys, key => key == "write_users");
  }
  [Fact(DisplayName = "It should remove custom attribute if it is found.")]
  public void It_should_remove_custom_attribute_if_it_is_found()
  {
    _user.SetCustomAttribute("read_users", "true");
    _user.SetCustomAttribute("write_users", "true");

    _user.RemoveCustomAttribute("   write_users ");
    Assert.Contains(_user.CustomAttributes.Keys, key => key == "read_users");
    Assert.DoesNotContain(_user.CustomAttributes.Keys, key => key == "write_users");

    UserModifiedEvent e = (UserModifiedEvent)_user.Changes.Last(e => e is UserModifiedEvent);
    Assert.Null(e.CustomAttributes["write_users"]);
  }

  [Fact(DisplayName = "It should not remove external identifier if it is not found.")]
  public void It_should_not_remove_external_identifier_if_it_is_not_found()
  {
    _user.SetExternalIdentifier("ActorId", "48c027ad-fb72-40a2-980b-6913a66816c0");

    _user.RemoveExternalIdentifier("OtherId");
    Assert.Contains(_user.ExternalIdentifiers.Keys, key => key == "ActorId");
    Assert.DoesNotContain(_user.ExternalIdentifiers.Keys, key => key == "OtherId");

    Assert.DoesNotContain(_user.Changes, e => e is UserExternalIdentifierChangedEvent change && change.Key == "OtherId");
  }
  [Fact(DisplayName = "It should remove external identifier if it is found.")]
  public void It_should_remove_external_identifier_if_it_is_found()
  {
    _user.SetExternalIdentifier("ActorId", "9302d60b-92b2-4268-82a8-7c5f35454a4c");
    _user.SetExternalIdentifier("OtherId", "f4635de0-f624-4d58-b2d9-67f43f7f518d");

    _user.RemoveExternalIdentifier("   OtherId ");
    Assert.Contains(_user.ExternalIdentifiers.Keys, key => key == "ActorId");
    Assert.DoesNotContain(_user.ExternalIdentifiers.Keys, key => key == "OtherId");

    Assert.Contains(_user.Changes, e => e is UserExternalIdentifierChangedEvent change
      && change.Key == "OtherId" && change.Value == null);
  }

  [Theory(DisplayName = "It should set the correct custom attribute.")]
  [InlineData(" ProfileId ", "   76   ")]
  public void It_should_set_the_correct_custom_attribute(string key, string value)
  {
    _user.SetCustomAttribute(key, value);
    Assert.Equal(value.Trim(), _user.CustomAttributes[key.Trim()]);

    UserModifiedEvent e = (UserModifiedEvent)_user.Changes.Last(e => e is UserModifiedEvent);
    Assert.Equal(value.Trim(), e.CustomAttributes[key.Trim()]);
  }
  [Fact(DisplayName = "It should set the correct custom attribute only if it is modified.")]
  public void It_should_set_the_correct_custom_attribute_only_if_it_is_modified()
  {
    _user.SetCustomAttribute("ProfileId", "51");
    UserModifiedEvent e = (UserModifiedEvent)_user.Changes.Last(e => e is UserModifiedEvent);
    Assert.Equal("51", e.CustomAttributes["ProfileId"]);

    _user.ClearChanges();
    _user.SetCustomAttribute("ProfileId", "51");
    Assert.DoesNotContain(_user.Changes, e => e is UserModifiedEvent);
  }

  [Theory(DisplayName = "It should set the correct external identifier.")]
  [InlineData(" ProfileId ", "   76   ")]
  public void It_should_set_the_correct_external_identifier(string key, string value)
  {
    _user.SetExternalIdentifier(key, value);
    Assert.Equal(value.Trim(), _user.ExternalIdentifiers[key.Trim()]);
    Assert.Contains(_user.Changes, e => e is UserExternalIdentifierChangedEvent change
      && change.Key == key.Trim() && change.Value == value.Trim());
  }
  [Fact(DisplayName = "It should set the correct external identifier only if it is modified.")]
  public void It_should_set_the_correct_external_identifier_only_if_it_is_modified()
  {
    _user.SetExternalIdentifier("ProfileId", "51");
    Assert.Contains(_user.Changes, e => e is UserExternalIdentifierChangedEvent change
      && change.Key == "ProfileId" && change.Value == "51");

    _user.ClearChanges();
    _user.SetExternalIdentifier("ProfileId", "51");
    Assert.DoesNotContain(_user.Changes, e => e is UserExternalIdentifierChangedEvent);
  }

  [Theory(DisplayName = "It should set the correct UniqueName.")]
  [InlineData(UniqueName)]
  [InlineData("  admin  ")]
  public void It_should_set_the_correct_UniqueName(string uniqueName)
  {
    _user.SetUniqueName(_uniqueNameSettings, uniqueName);
    Assert.Equal(uniqueName.Trim(), _user.UniqueName);
  }
  [Fact(DisplayName = "It should set the correct UniqueName only if it is modified.")]
  public void It_should_set_the_correct_UniqueName_only_if_it_is_modified()
  {
    _user.SetUniqueName(_uniqueNameSettings, "guest");
    Assert.Contains(_user.Changes, e => e is UserUniqueNameChangedEvent);

    _user.ClearChanges();
    _user.SetUniqueName(_uniqueNameSettings, "guest");
    Assert.DoesNotContain(_user.Changes, e => e is UserUniqueNameChangedEvent);
  }

  [Fact(DisplayName = "It should throw InvalidCredentialsException when changed password and current is not valid.")]
  public void It_should_throw_InvalidCredentialsException_when_changed_password_and_current_is_not_valid()
  {
    string current = "Test123!";
    _user.ChangePassword(_passwordSettings, current);
    Assert.Throws<InvalidCredentialsException>(() => _user.ChangePassword(_passwordSettings, current, current[1..]));
  }

  [Fact(DisplayName = "It should throw ValidationException when custom attribute key is not valid.")]
  public void It_should_throw_ValidationException_when_custom_attribute_key_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _user.SetCustomAttribute(string.Empty, "true"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));
  }
  [Fact(DisplayName = "It should throw ValidationException when custom attribute value is not valid.")]
  public void It_should_throw_ValidationException_when_custom_attribute_value_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _user.SetCustomAttribute("write_users", string.Empty));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("Value", failure.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when external identifier key is not valid.")]
  public void It_should_throw_ValidationException_when_external_identifier_key_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _user.SetExternalIdentifier(string.Empty, "ab57a5de-554f-48b7-813c-6a12fc7e9c19\r\n"));
    Assert.All(exception.Errors, e => Assert.Equal("Key", e.PropertyName));
  }
  [Fact(DisplayName = "It should throw ValidationException when external identifier value is not valid.")]
  public void It_should_throw_ValidationException_when_external_identifier_value_is_not_valid()
  {
    string value = new Bogus.Faker().Random.String(300, minChar: 'a', maxChar: 'z');
    var exception = Assert.Throws<ValidationException>(() => _user.SetExternalIdentifier("ProfileId", value));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("Value", failure.PropertyName);
  }

  [Fact(DisplayName = "It should throw ValidationException when password is not valid.")]
  public void It_should_throw_ValidationException_when_password_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _user.ChangePassword(_passwordSettings, string.Empty));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "NotEmptyValidator");
  }

  [Fact(DisplayName = "It should throw ValidationException when unique name is not valid.")]
  public void It_should_throw_ValidationException_when_unique_name_is_not_valid()
  {
    var exception = Assert.Throws<ValidationException>(() => _user.SetUniqueName(_uniqueNameSettings, "admin!"));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("AllowedCharactersValidator", failure.ErrorCode);
    Assert.Equal("UniqueName", failure.PropertyName);
  }

  private static Pbkdf2? GetPassword(UserAggregate user)
  {
    FieldInfo? passwordField = typeof(UserAggregate).GetField("_password", BindingFlags.Instance | BindingFlags.NonPublic);
    Assert.NotNull(passwordField);

    return passwordField.GetValue(user) as Pbkdf2;
  }
}
