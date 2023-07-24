using Logitar.Identity.Core;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Logitar.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.IntegrationTests.Sessions;

[Trait(Traits.Category, Categories.Integration)]
public class SessionServiceTests : IntegrationTestingBase
{
  private const string Password = "G_rw)XW5?Z7>C$~9";

  private readonly ISessionService _sessionService;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  private readonly UserAggregate _user;

  public SessionServiceTests() : base()
  {
    _sessionService = ServiceProvider.GetRequiredService<ISessionService>();
    _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
    _userSettings = ServiceProvider.GetRequiredService<IOptions<UserSettings>>();

    UserSettings userSettings = _userSettings.Value;
    _user = new(userSettings.UniqueNameSettings, uniqueName: "admin", tenantId: Guid.NewGuid().ToString())
    {
      Email = new EmailAddress(Faker.Person.Email, isVerified: true)
    };
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct Session.")]
  public async Task CreateAsync_it_should_create_the_correct_Session()
  {
    CreateSessionPayload payload = new()
    {
      UserId = _user.Id.Value,
      IsPersistent = true
    };
    Session session = await _sessionService.CreateAsync(payload, CancellationToken);
    Assert.Equal(payload.UserId, session.User.Id);
    Assert.Equal(payload.IsPersistent, session.IsPersistent);
    Assert.Equal(session.User.AuthenticatedOn, session.CreatedOn);
    AssertIsActive(session);
    await AssertRefreshTokenIsValidAsync(session);
  }

  [Fact(DisplayName = "CreateAsync: it should throw AggregateNotFoundException when user is not found.")]
  public async Task CreateAsync_it_should_throw_AggregateNotFoundException_when_user_is_not_found()
  {
    CreateSessionPayload payload = new()
    {
      UserId = Guid.Empty.ToString()
    };
    var exception = await Assert.ThrowsAsync<AggregateNotFoundException<UserAggregate>>(
      async () => await _sessionService.CreateAsync(payload, CancellationToken));
    Assert.Equal(payload.UserId, exception.Id);
    Assert.Equal("UserId", exception.PropertyName);
  }

  [Fact(DisplayName = "CreateAsync: it should throw UserIsDisabledException when user is disabled.")]
  public async Task CreateAsync_it_should_throw_UserIsDisabledException_when_user_is_disabled()
  {
    _user.Disable();
    await _userRepository.SaveAsync(_user);

    CreateSessionPayload payload = new()
    {
      UserId = _user.Id.Value,
    };
    var exception = await Assert.ThrowsAsync<UserIsDisabledException>(
      async () => await _sessionService.CreateAsync(payload, CancellationToken));
    Assert.Equal(_user.ToString(), exception.User);
  }

  [Fact(DisplayName = "CreateAsync: it should throw UserIsNotConfirmedException when user is not confirmed.")]
  public async Task CreateAsync_it_should_throw_UserIsNotConfirmedException_when_user_is_not_confirmed()
  {
    _user.Email = null;
    await _userRepository.SaveAsync(_user);

    CreateSessionPayload payload = new()
    {
      UserId = _user.Id.Value,
    };
    var exception = await Assert.ThrowsAsync<UserIsNotConfirmedException>(
      async () => await _sessionService.CreateAsync(payload, CancellationToken));
    Assert.Equal(_user.ToString(), exception.User);
  }

  [Fact(DisplayName = "SignInAsync: it should sign in the correct user using EmailAddress.")]
  public async Task SignInAsync_it_should_sign_in_the_correct_user_using_EmailAddress()
  {
    _user.SetPassword(_userSettings.Value.PasswordSettings, Password);
    await _userRepository.SaveAsync(_user);

    Assert.NotNull(_user.Email);
    string[] parts = _user.Email.Address.Split('@');
    string emailAddress = string.Join('@', parts[0].ToLower(), parts[1].ToUpper());
    SignInPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = emailAddress,
      Password = Password
    };
    Session session = await _sessionService.SignInAsync(payload, CancellationToken);
    Assert.Equal(_user.Id.Value, session.User.Id);
    Assert.Equal(payload.IsPersistent, session.IsPersistent);
    Assert.Equal(session.User.AuthenticatedOn, session.CreatedOn);
    AssertIsActive(session);
  }

  [Fact(DisplayName = "SignInAsync: it should sign in the correct user using UniqueName.")]
  public async Task SignInAsync_it_should_sign_in_the_correct_user_using_UniqueName()
  {
    _user.SetPassword(_userSettings.Value.PasswordSettings, Password);
    await _userRepository.SaveAsync(_user);

    SignInPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = "  AdmIn  ",
      Password = Password,
      IsPersistent = true
    };
    Session session = await _sessionService.SignInAsync(payload, CancellationToken);
    Assert.Equal(_user.Id.Value, session.User.Id);
    Assert.Equal(payload.IsPersistent, session.IsPersistent);
    Assert.Equal(session.User.AuthenticatedOn, session.CreatedOn);
    AssertIsActive(session);
    await AssertRefreshTokenIsValidAsync(session);
  }

  [Fact(DisplayName = "SignInAsync: it should throw InvalidCredentialsException when password is not valid.")]
  public async Task SignInAsync_it_should_throw_InvalidCredentialsException_when_password_is_not_valid()
  {
    _user.SetPassword(_userSettings.Value.PasswordSettings, Password);
    await _userRepository.SaveAsync(_user);

    SignInPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = _user.UniqueName,
      Password = "AAaa!!11"
    };
    await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _sessionService.SignInAsync(payload, CancellationToken));
  }

  [Fact(DisplayName = "SignInAsync: it should throw InvalidCredentialsException when user has no password.")]
  public async Task SignInAsync_it_should_throw_InvalidCredentialsException_when_user_has_no_password()
  {
    SignInPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = _user.UniqueName,
      Password = Password
    };
    await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _sessionService.SignInAsync(payload, CancellationToken));
  }

  [Fact(DisplayName = "SignInAsync: it should throw InvalidCredentialsException when user is not found.")]
  public async Task SignInAsync_it_should_throw_InvalidCredentialsException_when_user_is_not_found()
  {
    SignInPayload payload = new()
    {
      TenantId = Guid.Empty.ToString(),
      UniqueName = _user.UniqueName
    };
    await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _sessionService.SignInAsync(payload, CancellationToken));
  }

  [Fact(DisplayName = "SignInAsync: it should throw UserIsDisabledException when user is disabled.")]
  public async Task SignInAsync_it_should_throw_UserIsDisabledException_when_user_is_disabled()
  {
    _user.Disable();
    await _userRepository.SaveAsync(_user);

    SignInPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = _user.UniqueName
    };
    var exception = await Assert.ThrowsAsync<UserIsDisabledException>(
      async () => await _sessionService.SignInAsync(payload, CancellationToken));
    Assert.Equal(_user.ToString(), exception.User);
  }

  [Fact(DisplayName = "SignInAsync: it should throw UserIsNotConfirmedException when user is not confirmed.")]
  public async Task SignInAsync_it_should_throw_UserIsNotConfirmedException_when_user_is_not_confirmed()
  {
    _user.Email = null;
    await _userRepository.SaveAsync(_user);

    SignInPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = _user.UniqueName
    };
    var exception = await Assert.ThrowsAsync<UserIsNotConfirmedException>(
      async () => await _sessionService.SignInAsync(payload, CancellationToken));
    Assert.Equal(_user.ToString(), exception.User);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _userRepository.SaveAsync(_user);
  }

  private static void AssertIsActive(Session session)
  {
    Assert.True(session.IsActive);
    Assert.Null(session.SignedOutBy);
    Assert.Null(session.SignedOutOn);
  }
  private async Task AssertRefreshTokenIsValidAsync(Session session)
  {
    SessionEntity? entity = await IdentityContext.Sessions.SingleOrDefaultAsync(x => x.AggregateId == session.Id);
    Assert.NotNull(entity);
    Assert.NotNull(entity.Secret);
    Pbkdf2 secret = Pbkdf2.Parse(entity.Secret);

    Assert.NotNull(session.RefreshToken);
    string[] values = session.RefreshToken.Split('.');
    Assert.Equal("RT", values[0]);
    Assert.Equal(session.Id, values[1].ToUriSafeBase64());
    Assert.True(secret.IsMatch(Convert.FromBase64String(values[2].FromUriSafeBase64())));
  }
}
