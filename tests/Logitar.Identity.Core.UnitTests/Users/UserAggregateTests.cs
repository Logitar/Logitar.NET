using FluentValidation;
using FluentValidation.Results;
using Logitar.EventSourcing;
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
