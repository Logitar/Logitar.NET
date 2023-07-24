using FluentValidation;
using FluentValidation.Results;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Users;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Logitar.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.IntegrationTests.Users;

[Trait(Traits.Category, Categories.Integration)]
public class UserServiceTests : IntegrationTestingBase
{
  private readonly IUserRepository _userRepository;
  private readonly IUserService _userService;
  private readonly IOptions<UserSettings> _userSettings;

  private readonly UserAggregate _user;

  public UserServiceTests() : base()
  {
    _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
    _userService = ServiceProvider.GetRequiredService<IUserService>();
    _userSettings = ServiceProvider.GetRequiredService<IOptions<UserSettings>>();

    UserSettings userSettings = _userSettings.Value;
    _user = new(userSettings.UniqueNameSettings, uniqueName: "admin", tenantId: Guid.NewGuid().ToString())
    {
      Email = new(Faker.Person.Email, isVerified: true)
    };
  }

  [Fact(DisplayName = "AuthenticateAsync: it authenticates the correct user.")]
  public async Task AuthenticateAsync_it_authenticates_the_correct_user()
  {
    AuthenticateUserPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = _user.UniqueName,
      Password = "Test123!"
    };

    _user.SetPassword(_userSettings.Value.PasswordSettings, payload.Password);
    await _userRepository.SaveAsync(_user);

    Assert.Null(_user.AuthenticatedOn);

    User user = await _userService.AuthenticateAsync(payload, CancellationToken);
    Assert.Equal(_user.Id.Value, user.Id);
    Assert.NotNull(user.AuthenticatedOn);
  }

  [Fact(DisplayName = "AuthenticateAsync: it throws InvalidCredentialsException when password is not valid.")]
  public async Task AuthenticateAsync_it_throws_InvalidCredentialsException_when_passord_is_not_valid()
  {
    AuthenticateUserPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = _user.UniqueName,
      Password = "Test123!"
    };
    var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _userService.AuthenticateAsync(payload, CancellationToken));
    Assert.StartsWith("The specified password does not match the user.", exception.Message);
  }

  [Fact(DisplayName = "AuthenticateAsync: it throws InvalidCredentialsException when user is not found.")]
  public async Task AuthenticateAsync_it_throws_InvalidCredentialsException_when_user_is_not_found()
  {
    AuthenticateUserPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = $"{_user.UniqueName}2"
    };
    var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _userService.AuthenticateAsync(payload, CancellationToken));
    Assert.StartsWith("The specified user could not be found.", exception.Message);
  }

  [Fact(DisplayName = "AuthenticateAsync: it throws UserIsDisabledException when user is disabled.")]
  public async Task AuthenticateAsync_it_throws_UserIsDisabledException_when_user_is_disabled()
  {
    _user.Disable();
    await _userRepository.SaveAsync(_user);

    AuthenticateUserPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = _user.UniqueName,
      Password = "Test123!"
    };
    var exception = await Assert.ThrowsAsync<UserIsDisabledException>(
      async () => await _userService.AuthenticateAsync(payload, CancellationToken));
    Assert.Equal(_user.ToString(), exception.User);
  }

  [Fact(DisplayName = "AuthenticateAsync: it throws UserIsNotConfirmedException when user is not confirmed.")]
  public async Task AuthenticateAsync_it_throws_UserIsNotConfirmedException_when_user_is_not_confirmed()
  {
    _user.Email = null;
    await _userRepository.SaveAsync(_user);

    AuthenticateUserPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = _user.UniqueName,
      Password = "Test123!"
    };
    var exception = await Assert.ThrowsAsync<UserIsNotConfirmedException>(
      async () => await _userService.AuthenticateAsync(payload, CancellationToken));
    Assert.Equal(_user.ToString(), exception.User);
  }

  [Fact(DisplayName = "ChangePasswordAsync: it should change the correct password.")]
  public async Task ChangePasswordAsync_it_should_change_the_correct_password()
  {
    UserSettings userSettings = _userSettings.Value;
    _user.SetPassword(userSettings.PasswordSettings, "Test123!");
    await _userRepository.SaveAsync(_user);

    ChangePasswordPayload payload = new()
    {
      Current = "Test123!",
      Password = "yEmZS(C@W39+"
    };
    User? user = await _userService.ChangePasswordAsync(_user.Id.Value, payload, CancellationToken);
    Assert.NotNull(user);
    Assert.NotNull(user.PasswordChangedOn);
    Assert.True(user.HasPassword);
    Assert.Equal(user.Id, user.PasswordChangedBy?.Id);

    UserEntity? entity = await IdentityContext.Users.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == user.Id);
    Assert.NotNull(entity);
    Assert.NotNull(entity.Password);
    Assert.True(Pbkdf2.TryParse(entity.Password, out Pbkdf2? pbkdf2));
    Assert.NotNull(pbkdf2);
    Assert.True(pbkdf2.IsMatch(payload.Password));
  }

  [Fact(DisplayName = "ChangePasswordAsync: it should return null when user is not found.")]
  public async Task ChangePasswordAsync_it_should_return_null_when_user_is_not_found()
  {
    ChangePasswordPayload payload = new();
    User? user = await _userService.ChangePasswordAsync(Guid.Empty.ToString(), payload, CancellationToken);
    Assert.Null(user);
  }

  [Fact(DisplayName = "ChangePasswordAsync: it should thrown InvalidCredentialsException when current password is not valid.")]
  public async Task ChangePasswordAsync_it_should_throw_InvalidCredentialsException_when_current_password_is_not_valid()
  {
    UserSettings userSettings = _userSettings.Value;
    _user.SetPassword(userSettings.PasswordSettings, "Test123!");
    await _userRepository.SaveAsync(_user);

    ChangePasswordPayload payload = new()
    {
      Current = "AAaa!!11"
    };
    await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _userService.ChangePasswordAsync(_user.Id.Value, payload, CancellationToken));
  }

  [Fact(DisplayName = "ChangePasswordAsync: it should throw InvalidCredentialsException when user has no password.")]
  public async Task ChangePasswordAsync_it_should_throw_InvalidCredentialsException_when_user_has_no_password()
  {
    ChangePasswordPayload payload = new();
    await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _userService.ChangePasswordAsync(_user.Id.Value, payload, CancellationToken));
  }

  [Fact(DisplayName = "ChangePasswordAsync: it should throw ValidationException when new password is not valid.")]
  public async Task ChangePasswordAsync_it_should_throw_ValidationException_when_new_password_is_not_valid()
  {
    UserSettings userSettings = _userSettings.Value;
    _user.SetPassword(userSettings.PasswordSettings, "Test123!");
    await _userRepository.SaveAsync(_user);

    ChangePasswordPayload payload = new()
    {
      Current = "Test123!",
      Password = "AAaa!!11"
    };
    var exception = await Assert.ThrowsAsync<ValidationException>(
      async () => await _userService.ChangePasswordAsync(_user.Id.Value, payload, CancellationToken));
    ValidationFailure failure = exception.Errors.Single();
    Assert.Equal("PasswordRequiresUniqueChars", failure.ErrorCode);
    Assert.Equal("Password", failure.PropertyName);
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct user.")]
  public async Task CreateAsync_it_should_create_the_correct_user()
  {
    Assert.NotNull(_user.Email);
    string[] emailParts = _user.Email.Address.Split('@');
    emailParts[0] = $"{emailParts[0]}2";
    string emailAddress = string.Join('@', emailParts);
    CreateUserPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = $"{_user.UniqueName}2",
      Password = "Test123!",
      IsDisabled = true,
      Email = new CreateEmailPayload
      {
        Address = emailAddress,
        IsVerified = _user.Email.IsVerified
      },
      FirstName = "Charles",
      LastName = "Raymond",
      Locale = "fr-CA"
    };
    User user = await _userService.CreateAsync(payload, CancellationToken);
    Assert.NotNull(user.Email);
    Assert.Equal(payload.TenantId, user.TenantId);
    Assert.Equal(payload.UniqueName, user.UniqueName);
    Assert.Equal(payload.Password != null, user.HasPassword);
    Assert.Equal(Actor, user.PasswordChangedBy);
    Assert.Equal(payload.IsDisabled, user.IsDisabled);
    Assert.Equal(Actor, user.DisabledBy);
    Assert.Equal(emailAddress, user.Email.Address);
    Assert.Equal(payload.Email.IsVerified, user.Email.IsVerified);
    Assert.Equal(payload.Email.IsVerified, user.IsConfirmed);
    Assert.Equal(payload.FirstName, user.FirstName);
    Assert.Equal(payload.LastName, user.LastName);
    Assert.Equal(PersonHelper.BuildFullName(payload.FirstName, payload.LastName), user.FullName);
    Assert.Equal(payload.Locale, user.Locale);
    Assert.NotNull(user.PasswordChangedOn);
    Assert.NotNull(user.DisabledOn);
  }

  [Fact(DisplayName = "CreateAsync: it should throw EmailAddressAlreadyUsedException when email address is already used.")]
  public async Task CreateAsync_it_should_throw_EmailAddressAlreadyUsedException_when_email_address_is_already_used()
  {
    Assert.NotNull(_user.Email);
    CreateUserPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = $"{_user.UniqueName}2",
      Email = new CreateEmailPayload
      {
        Address = _user.Email.Address,
        IsVerified = _user.Email.IsVerified
      }
    };
    var exception = await Assert.ThrowsAsync<EmailAddressAlreadyUsedException>(
      async () => await _userService.CreateAsync(payload, CancellationToken));
    Assert.Equal(payload.TenantId, exception.TenantId);
    Assert.Equal(payload.Email.Address, exception.EmailAddress);
    Assert.Equal("Email", exception.PropertyName);
  }

  [Fact(DisplayName = "CreateAsync: it should throw UniqueNameAlreadyUsedException when unique name is already used.")]
  public async Task CreateAsync_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    CreateUserPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = _user.UniqueName
    };
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<UserAggregate>>(
      async () => await _userService.CreateAsync(payload, CancellationToken));
    Assert.Equal(payload.TenantId, exception.TenantId);
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact(DisplayName = "DeleteAsync: it should delete the correct user.")]
  public async Task DeleteAsync_it_should_delete_the_correct_user()
  {
    User? user = await _userService.DeleteAsync(_user.Id.Value, CancellationToken);
    Assert.NotNull(user);
    Assert.Equal(_user.Id.Value, user.Id);

    UserEntity? entity = await IdentityContext.Users.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _user.Id.Value);
    Assert.Null(entity);
  }

  [Fact(DisplayName = "DeleteAsync: it should return null when user is not found.")]
  public async Task DeleteAsync_it_should_return_null_when_user_is_not_found()
  {
    User? user = await _userService.DeleteAsync(Guid.Empty.ToString(), CancellationToken);
    Assert.Null(user);
  }

  [Fact(DisplayName = "ReadAsync: it should read the correct user.")]
  public async Task ReadAsync_it_should_read_the_correct_user()
  {
    User? user = await _userService.ReadAsync(_user.Id.Value, _user.TenantId, _user.UniqueName, CancellationToken);
    Assert.NotNull(user);
    Assert.Equal(_user.Id.Value, user.Id);
  }

  [Fact(DisplayName = "ReadAsync: it should read the correct user by EmailAddress.")]
  public async Task ReadAsync_it_should_read_the_correct_user_by_EmailAddress()
  {
    Assert.NotNull(_user.Email);
    User? user = await _userService.ReadAsync(id: null, _user.TenantId, _user.Email.Address, CancellationToken);
    Assert.NotNull(user);
    Assert.Equal(_user.Id.Value, user.Id);
  }

  [Fact(DisplayName = "ReadAsync: it should return null when user is not found.")]
  public async Task ReadAsync_it_should_return_null_when_user_is_not_found()
  {
    User? user = await _userService.ReadAsync(id: Guid.Empty.ToString(), cancellationToken: CancellationToken);
    Assert.Null(user);
  }

  [Fact(DisplayName = "ReadAsync: it should throw TooManyResultsException when there are too many results.")]
  public async Task ReadAsync_it_should_throw_TooManyResultsException_when_there_are_too_many_results()
  {
    UserSettings userSettings = _userSettings.Value;
    UserAggregate other = new(userSettings.UniqueNameSettings, _user.UniqueName, tenantId: null);
    await _userRepository.SaveAsync(other);

    var exception = await Assert.ThrowsAsync<TooManyResultsException<User>>(
      async () => await _userService.ReadAsync(_user.Id.Value, tenantId: null, other.UniqueName, CancellationToken));
    Assert.Equal(1, exception.Expected);
    Assert.Equal(2, exception.Actual);
  }

  [Fact(DisplayName = "UpdateAsync: it should return null when user is not found.")]
  public async Task UpdateAsync_it_should_return_null_when_user_is_not_found()
  {
    UpdateUserPayload payload = new();
    User? user = await _userService.UpdateAsync(Guid.Empty.ToString(), payload, CancellationToken);
    Assert.Null(user);
  }

  [Fact(DisplayName = "UpdateAsync: it should throw EmailAddressAlreadyUsedException when unique name is already used.")]
  public async Task UpdateAsync_it_should_throw_EmailAddressAlreadyUsedException_when_unique_name_is_already_used()
  {
    CreateUserPayload createPayload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = "test"
    };
    User user = await _userService.CreateAsync(createPayload, CancellationToken);

    Assert.NotNull(_user.Email);
    UpdateUserPayload updatePayload = new()
    {
      Email = new MayBe<UpdateEmailPayload>(new UpdateEmailPayload
      {
        Address = _user.Email.Address,
        IsVerified = _user.Email.IsVerified
      })
    };
    Assert.NotNull(updatePayload.Email.Value);
    var exception = await Assert.ThrowsAsync<EmailAddressAlreadyUsedException>(
      async () => await _userService.UpdateAsync(user.Id, updatePayload, CancellationToken));
    Assert.Equal(user.TenantId, exception.TenantId);
    Assert.Equal(updatePayload.Email.Value.Address, exception.EmailAddress);
    Assert.Equal("Email", exception.PropertyName);
  }

  [Fact(DisplayName = "UpdateAsync: it should throw UniqueNameAlreadyUsedException when unique name is already used.")]
  public async Task UpdateAsync_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    CreateUserPayload createPayload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = "test"
    };
    User user = await _userService.CreateAsync(createPayload, CancellationToken);

    UpdateUserPayload updatePayload = new()
    {
      UniqueName = _user.UniqueName.ToUpper()
    };
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<UserAggregate>>(
      async () => await _userService.UpdateAsync(user.Id, updatePayload, CancellationToken));
    Assert.Equal(user.TenantId, exception.TenantId);
    Assert.Equal(updatePayload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact(DisplayName = "UpdateAsync: it should update the correct user.")]
  public async Task UpdateAsync_it_should_update_the_correct_user()
  {
    Assert.False(_user.HasPassword);
    Assert.NotNull(_user.Email);
    UpdateUserPayload payload = new()
    {
      UniqueName = $"{_user.UniqueName}2",
      Password = "Test123!",
      IsDisabled = true,
      Email = new MayBe<UpdateEmailPayload>(new UpdateEmailPayload
      {
        Address = Faker.Internet.Email()
      }),
      FirstName = new MayBe<string>(Faker.Person.FirstName),
      LastName = new MayBe<string>(Faker.Person.LastName),
      Locale = new MayBe<string>("en-US")
    };
    Assert.NotNull(payload.Email.Value);
    User? user = await _userService.UpdateAsync(_user.Id.Value, payload, CancellationToken);
    Assert.NotNull(user);
    Assert.NotNull(user.Email);
    Assert.Equal(payload.UniqueName, user.UniqueName);
    Assert.Equal(payload.IsDisabled, user.IsDisabled);
    Assert.Equal(payload.Email.Value.Address, user.Email.Address);
    Assert.Equal(payload.Email.Value.IsVerified ?? _user.Email.IsVerified, user.Email.IsVerified);
    Assert.Equal(payload.FirstName.Value, user.FirstName);
    Assert.Equal(payload.LastName.Value, user.LastName);
    Assert.Equal(payload.Locale.Value, user.Locale);
    Assert.True(user.HasPassword);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _userRepository.SaveAsync(_user);
  }
}
