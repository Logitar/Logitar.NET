using FluentValidation;
using FluentValidation.Results;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Payloads;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Users;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Sessions;
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
  private readonly ISessionRepository _sessionRepository;
  private readonly IUserRepository _userRepository;
  private readonly IUserService _userService;
  private readonly IOptions<UserSettings> _userSettings;

  private readonly UserAggregate _user;

  public UserServiceTests() : base()
  {
    _sessionRepository = ServiceProvider.GetRequiredService<ISessionRepository>();
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
      Address = new CreateAddressPayload
      {
        Street = "1909 Av. des Canadiens-de-Montréal",
        Locality = "Montréal",
        Country = "CA",
        Region = "QC",
        PostalCode = "H3B 5E8"
      },
      Email = new CreateEmailPayload
      {
        Address = emailAddress,
        IsVerified = _user.Email.IsVerified
      },
      Phone = new CreatePhonePayload
      {
        CountryCode = "CA",
        Number = "+15149322582",
        Extension = "4232",
        IsVerified = false
      },
      FirstName = "Charles",
      MiddleName = "Robert",
      LastName = "Raymond",
      Nickname = "Bob",
      Birthdate = DateTime.UtcNow.AddYears(-25),
      Gender = "Male",
      Locale = "fr-CA",
      TimeZone = "America/Toronto",
      Picture = "https://www.test.com/assets/img/profile.jpg",
      Profile = "    ",
      Website = "https://www.test.com/"
    };
    User user = await _userService.CreateAsync(payload, CancellationToken);
    Assert.NotNull(user.Address);
    Assert.NotNull(user.Email);
    Assert.NotNull(user.Phone);
    Assert.Equal(payload.TenantId, user.TenantId);
    Assert.Equal(payload.UniqueName, user.UniqueName);
    Assert.Equal(payload.Password != null, user.HasPassword);
    Assert.Equal(Actor, user.PasswordChangedBy);
    Assert.Equal(payload.IsDisabled, user.IsDisabled);
    Assert.Equal(Actor, user.DisabledBy);
    Assert.Equal(payload.Address.Street, user.Address.Street);
    Assert.Equal(payload.Address.Locality, user.Address.Locality);
    Assert.Equal(payload.Address.Country, user.Address.Country);
    Assert.Equal(payload.Address.Region, user.Address.Region);
    Assert.Equal(payload.Address.PostalCode, user.Address.PostalCode);
    Assert.Equal(PostalAddressHelper.Format(payload.Address), user.Address.Formatted);
    Assert.Equal(emailAddress, user.Email.Address);
    Assert.Equal(payload.Phone.CountryCode, user.Phone.CountryCode);
    Assert.Equal(payload.Phone.Number, user.Phone.Number);
    Assert.Equal(payload.Phone.Extension, user.Phone.Extension);
    Assert.Equal(payload.Phone.Number, user.Phone.E164Formatted);
    Assert.Equal(payload.Phone.IsVerified, user.Phone.IsVerified);
    Assert.Equal(payload.FirstName, user.FirstName);
    Assert.Equal(payload.MiddleName, user.MiddleName);
    Assert.Equal(payload.LastName, user.LastName);
    Assert.Equal(payload.Nickname, user.Nickname);
    Assert.Equal(payload.Birthdate.Value, user.Birthdate);
    Assert.Equal(payload.Gender?.ToLower(), user.Gender);
    Assert.Equal(payload.Locale, user.Locale);
    Assert.Equal(payload.TimeZone, user.TimeZone);
    Assert.Equal(payload.Picture, user.Picture);
    Assert.Equal(payload.Website, user.Website);
    Assert.Equal(PersonHelper.BuildFullName(payload.FirstName, payload.MiddleName, payload.LastName), user.FullName);
    Assert.NotNull(user.PasswordChangedOn);
    Assert.NotNull(user.DisabledOn);
    Assert.Null(user.Profile);

    bool isConfirmed = payload.Email.IsVerified || payload.Phone.IsVerified;
    Assert.Equal(isConfirmed, user.IsConfirmed);
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
    SessionAggregate session = _user.SignIn(_userSettings.Value);
    await _sessionRepository.SaveAsync(session);

    Assert.True(await IdentityContext.Sessions.AnyAsync());

    User? user = await _userService.DeleteAsync(_user.Id.Value, CancellationToken);
    Assert.NotNull(user);
    Assert.Equal(_user.Id.Value, user.Id);

    UserEntity? entity = await IdentityContext.Users.AsNoTracking()
      .SingleOrDefaultAsync(x => x.AggregateId == _user.Id.Value);
    Assert.Null(entity);

    Assert.False(await IdentityContext.Sessions.AnyAsync());
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

  [Fact(DisplayName = "ReplaceAsync: it should replace the correct user.")]
  public async Task ReplaceAsync_it_should_replace_the_correct_user()
  {
    bool isDisabled = _user.IsDisabled;

    ReplaceUserPayload payload = new()
    {
      UniqueName = $"{_user.UniqueName}2",
      Phone = new UpdatePhonePayload
      {
        CountryCode = "CA",
        Number = "+15149322582",
        Extension = "4232",
        IsVerified = true
      },
      FirstName = Faker.Person.FirstName,
      MiddleName = "Edward",
      LastName = Faker.Person.LastName,
      Nickname = "Eddy",
      Birthdate = DateTime.UtcNow.AddYears(-25),
      Gender = Faker.Person.Gender.ToString(),
      Locale = "en-US",
      TimeZone = "America/New_York",
      Picture = "https://www.test.com/assets/img/profile.jpg",
      Profile = "   ",
      Website = "https://www.test.com/"
    };
    User? user = await _userService.ReplaceAsync(_user.Id.Value, payload, CancellationToken);
    Assert.NotNull(user);
    Assert.Equal(payload.UniqueName, user.UniqueName);
    Assert.False(user.HasPassword);
    Assert.Equal(isDisabled, user.IsDisabled);
    Assert.Null(user.Address);
    Assert.Null(user.Email);
    Assert.Equal(payload.Phone?.FormatToE164(), user.Phone?.FormatToE164());
    Assert.True(user.IsConfirmed);
    Assert.Equal(payload.FirstName, user.FirstName);
    Assert.Equal(payload.MiddleName, user.MiddleName);
    Assert.Equal(payload.LastName, user.LastName);
    Assert.Equal(payload.Nickname, user.Nickname);
    Assert.Equal(payload.Birthdate, user.Birthdate);
    Assert.Equal(payload.Gender.ToLower(), user.Gender);
    Assert.Equal(payload.Locale, user.Locale);
    Assert.Equal(payload.TimeZone, user.TimeZone);
    Assert.Equal(payload.Picture, user.Picture);
    Assert.Null(user.Profile);
    Assert.Equal(payload.Website, user.Website);
  }

  [Fact(DisplayName = "ReplaceAsync: it should return null when user is not found.")]
  public async Task ReplaceAsync_it_should_return_null_when_user_is_not_found()
  {
    ReplaceUserPayload payload = new();
    User? user = await _userService.ReplaceAsync(Guid.Empty.ToString(), payload, CancellationToken);
    Assert.Null(user);
  }

  [Fact(DisplayName = "ReplaceAsync: it should throw EmailAddressAlreadyUsedException when unique name is already used.")]
  public async Task ReplaceAsync_it_should_throw_EmailAddressAlreadyUsedException_when_unique_name_is_already_used()
  {
    UserAggregate other = new(_userSettings.Value.UniqueNameSettings, "test", _user.TenantId);
    await _userRepository.SaveAsync(other);

    Assert.NotNull(_user.Email);
    ReplaceUserPayload payload = new()
    {
      Email = new UpdateEmailPayload
      {
        Address = _user.Email.Address,
        IsVerified = _user.Email.IsVerified
      }
    };
    var exception = await Assert.ThrowsAsync<EmailAddressAlreadyUsedException>(
      async () => await _userService.ReplaceAsync(other.Id.Value, payload, CancellationToken));
    Assert.Equal(other.TenantId, exception.TenantId);
    Assert.Equal(payload.Email.Address, exception.EmailAddress);
    Assert.Equal("Email", exception.PropertyName);
  }

  [Fact(DisplayName = "ReplaceAsync: it should throw UniqueNameAlreadyUsedException when unique name is already used.")]
  public async Task ReplaceAsync_replace_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    UserAggregate other = new(_userSettings.Value.UniqueNameSettings, "test", _user.TenantId);
    await _userRepository.SaveAsync(other);

    ReplaceUserPayload payload = new()
    {
      UniqueName = _user.UniqueName
    };
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<UserAggregate>>(
      async () => await _userService.ReplaceAsync(other.Id.Value, payload, CancellationToken));
    Assert.Equal(other.TenantId, exception.TenantId);
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact(DisplayName = "SearchAsync: it should return the correct search results.")]
  public async Task SearchAsync_it_should_return_the_correct_search_results()
  {
    UserSettings userSettings = _userSettings.Value;
    string tenantId = Guid.NewGuid().ToString();

    UserAggregate user1 = new(userSettings.UniqueNameSettings, "aadmin", tenantId);
    UserAggregate user2 = new(userSettings.UniqueNameSettings, "badmin", tenantId);
    UserAggregate user3 = new(userSettings.UniqueNameSettings, "cadmin", tenantId);
    UserAggregate disabled = new(userSettings.UniqueNameSettings, "dadmin", tenantId);
    disabled.Disable();
    await _userRepository.SaveAsync(new[] { user1, user2, user3, disabled });

    SearchUserPayload payload = new()
    {
      IsDisabled = false,
      Sort = new[]
      {
        new UserSortOption((UserSort)(-1)),
        new UserSortOption(UserSort.UniqueName, isDescending: true)
      },
      Skip = 1,
      Limit = -10
    };
    payload.Id.Operator = (SearchOperator)(-1);
    payload.Id.Terms = new[] { new SearchTerm(_user.Id.Value) };
    payload.Search.Terms = new[] { new SearchTerm("%ADMIN%") };
    payload.TenantId.Terms = new[] { new SearchTerm(tenantId) };

    SearchResults<User> results = await _userService.SearchAsync(payload, CancellationToken);
    Assert.Equal(3, results.Total);
    Assert.Equal(2, results.Items.Count());
    Assert.Equal(user2.Id.Value, results.Items.ElementAt(0).Id);
    Assert.Equal(user1.Id.Value, results.Items.ElementAt(1).Id);
  }

  [Fact(DisplayName = "SignOutAsync: it should return null when user is not found.")]
  public async Task SignOutAsync_it_should_return_null_when_user_is_not_found()
  {
    User? user = await _userService.SignOutAsync(Guid.Empty.ToString(), CancellationToken);
    Assert.Null(user);
  }

  [Fact(DisplayName = "SignOutAsync: it should sign out the correct sessions.")]
  public async Task SignOutAsync_it_should_sign_out_the_correct_sessions()
  {
    SessionAggregate session1 = new(_user);
    SessionAggregate session2 = new(_user);
    SessionAggregate signedOut = new(_user);
    signedOut.SignOut();
    await _sessionRepository.SaveAsync(new[] { session1, session2, signedOut });

    SessionEntity[] sessions = await IdentityContext.Sessions.AsNoTracking()
      .Include(x => x.User)
      .Where(x => x.User!.AggregateId == _user.Id.Value && x.IsActive)
      .ToArrayAsync();
    Assert.NotEmpty(sessions);

    User? user = await _userService.SignOutAsync(_user.Id.Value, CancellationToken);
    Assert.NotNull(user);
    Assert.Equal(_user.Id.Value, user.Id);

    sessions = await IdentityContext.Sessions.AsNoTracking()
      .Include(x => x.User)
      .Where(x => x.User!.AggregateId == _user.Id.Value && x.IsActive)
      .ToArrayAsync();
    Assert.Empty(sessions);
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
    UserAggregate other = new(_userSettings.Value.UniqueNameSettings, "test", _user.TenantId);
    await _userRepository.SaveAsync(other);

    Assert.NotNull(_user.Email);
    UpdateUserPayload payload = new()
    {
      Email = new MayBe<UpdateEmailPayload>(new UpdateEmailPayload
      {
        Address = _user.Email.Address,
        IsVerified = _user.Email.IsVerified
      })
    };
    Assert.NotNull(payload.Email.Value);
    var exception = await Assert.ThrowsAsync<EmailAddressAlreadyUsedException>(
      async () => await _userService.UpdateAsync(other.Id.Value, payload, CancellationToken));
    Assert.Equal(other.TenantId, exception.TenantId);
    Assert.Equal(payload.Email.Value.Address, exception.EmailAddress);
    Assert.Equal("Email", exception.PropertyName);
  }

  [Fact(DisplayName = "UpdateAsync: it should throw UniqueNameAlreadyUsedException when unique name is already used.")]
  public async Task UpdateAsync_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    UserAggregate other = new(_userSettings.Value.UniqueNameSettings, "test", _user.TenantId);
    await _userRepository.SaveAsync(other);

    UpdateUserPayload payload = new()
    {
      UniqueName = _user.UniqueName.ToUpper()
    };
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<UserAggregate>>(
      async () => await _userService.UpdateAsync(other.Id.Value, payload, CancellationToken));
    Assert.Equal(other.TenantId, exception.TenantId);
    Assert.Equal(payload.UniqueName, exception.UniqueName);
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
      Address = new MayBe<UpdateAddressPayload>(new UpdateAddressPayload
      {
        Street = "1909 Av. des Canadiens-de-Montréal",
        Locality = "Montréal",
        Country = "CA",
        Region = "QC",
        PostalCode = "H3B 5E8"
      }),
      Email = new MayBe<UpdateEmailPayload>(new UpdateEmailPayload
      {
        Address = Faker.Internet.Email()
      }),
      Phone = new MayBe<UpdatePhonePayload>(new UpdatePhonePayload
      {
        CountryCode = "CA",
        Number = "+15149322582",
        Extension = "4232"
      }),
      FirstName = new MayBe<string>(Faker.Person.FirstName),
      MiddleName = new MayBe<string>("Edward"),
      LastName = new MayBe<string>(Faker.Person.LastName),
      Nickname = new MayBe<string>("Eddy"),
      Birthdate = new MayBe<DateTime?>(DateTime.UtcNow.AddYears(-25)),
      Gender = new MayBe<string>(Faker.Person.Gender.ToString()),
      Locale = new MayBe<string>("en-US"),
      TimeZone = new MayBe<string>("America/New_York"),
      Picture = new MayBe<string>("https://www.test.com/assets/img/profile.jpg"),
      Profile = new MayBe<string>("   "),
      Website = new MayBe<string>("https://www.test.com/")
    };
    Assert.NotNull(payload.Address.Value);
    Assert.NotNull(payload.Email.Value);
    Assert.NotNull(payload.Phone.Value);
    User? user = await _userService.UpdateAsync(_user.Id.Value, payload, CancellationToken);
    Assert.NotNull(user);
    Assert.NotNull(user.Address);
    Assert.NotNull(user.Email);
    Assert.NotNull(user.Phone);
    Assert.Equal(payload.UniqueName, user.UniqueName);
    Assert.Equal(payload.IsDisabled, user.IsDisabled);
    Assert.Equal(payload.Address.Value.Street, user.Address.Street);
    Assert.Equal(payload.Address.Value.Locality, user.Address.Locality);
    Assert.Equal(payload.Address.Value.Country, user.Address.Country);
    Assert.Equal(payload.Address.Value.Region, user.Address.Region);
    Assert.Equal(payload.Address.Value.PostalCode, user.Address.PostalCode);
    Assert.Equal(PostalAddressHelper.Format(payload.Address.Value), user.Address.Formatted);
    Assert.Equal(payload.Email.Value.Address, user.Email.Address);
    Assert.Equal(payload.Email.Value.IsVerified ?? _user.Email?.IsVerified ?? false, user.Email.IsVerified);
    Assert.Equal(payload.Phone.Value.CountryCode, user.Phone.CountryCode);
    Assert.Equal(payload.Phone.Value.Number, user.Phone.Number);
    Assert.Equal(payload.Phone.Value.Extension, user.Phone.Extension);
    Assert.Equal(payload.Phone.Value.Number, user.Phone.E164Formatted);
    Assert.Equal(payload.Phone.Value.IsVerified ?? _user.Phone?.IsVerified ?? false, user.Phone.IsVerified);
    Assert.Equal(payload.FirstName.Value, user.FirstName);
    Assert.Equal(payload.MiddleName.Value, user.MiddleName);
    Assert.Equal(payload.LastName.Value, user.LastName);
    Assert.Equal(payload.Nickname.Value, user.Nickname);
    Assert.Equal(payload.Birthdate.Value, user.Birthdate);
    Assert.Equal(payload.Gender.Value?.ToLower(), user.Gender);
    Assert.Equal(payload.Locale.Value, user.Locale);
    Assert.Equal(payload.TimeZone.Value, user.TimeZone);
    Assert.Equal(payload.Picture.Value, user.Picture);
    Assert.Equal(payload.Website.Value, user.Website);
    Assert.Equal(PersonHelper.BuildFullName(payload.FirstName.Value, payload.MiddleName.Value,
      payload.LastName.Value), user.FullName);
    Assert.Null(user.Profile);
    Assert.True(user.HasPassword);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _userRepository.SaveAsync(_user);
  }
}
