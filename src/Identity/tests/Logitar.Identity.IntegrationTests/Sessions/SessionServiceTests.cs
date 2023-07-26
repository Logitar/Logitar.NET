using Logitar.Identity.Core;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Payloads;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Sessions;
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

  private readonly ISessionRepository _sessionRepository;
  private readonly ISessionService _sessionService;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  private readonly UserAggregate _user;

  public SessionServiceTests() : base()
  {
    _sessionRepository = ServiceProvider.GetRequiredService<ISessionRepository>();
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
    Assert.Equal(session.User.Id, session.CreatedBy.Id);
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

  [Fact(DisplayName = "ReadAsync: it should read the correct session.")]
  public async Task ReadAsync_it_should_read_the_correct_session()
  {
    SessionAggregate aggregate = _user.SignIn(_userSettings.Value);
    await _sessionRepository.SaveAsync(aggregate);

    Session? session = await _sessionService.ReadAsync(id: aggregate.Id.Value, cancellationToken: CancellationToken);
    Assert.NotNull(session);
    Assert.Equal(aggregate.Id.Value, session.Id);
  }

  [Fact(DisplayName = "ReadAsync: it should return null when session is not found.")]
  public async Task ReadAsync_it_should_return_null_when_session_is_not_found()
  {
    Session? session = await _sessionService.ReadAsync(id: Guid.Empty.ToString(), cancellationToken: CancellationToken);
    Assert.Null(session);
  }

  [Fact(DisplayName = "RenewAsync: it should renew the correct session.")]
  public async Task RenewAsync_it_should_renew_the_correct_session()
  {
    SessionAggregate aggregate = new(_user, isPersistent: true);
    await _sessionRepository.SaveAsync(aggregate);

    RenewPayload payload = new()
    {
      RefreshToken = new RefreshToken(aggregate).ToString()
    };
    Session session = await _sessionService.RenewAsync(payload, CancellationToken);
    Assert.Equal(_user.Id.Value, session.User.Id);
    Assert.Equal(session.User.Id, session.UpdatedBy.Id);
    Assert.True(session.IsPersistent);
    AssertIsActive(session);
    await AssertRefreshTokenIsValidAsync(session);
  }

  [Fact(DisplayName = "RenewAsync: it should throw InvalidCredentialsException when refresh token is not valid.")]
  public async Task RenewAsync_it_should_throw_InvalidCredentialsException_when_refresh_token_is_not_valid()
  {
    RenewPayload payload = new()
    {
      RefreshToken = "ID.abc.123"
    };
    var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _sessionService.RenewAsync(payload, CancellationToken));
    Assert.StartsWith($"The refresh token '{payload.RefreshToken}' is not valid.", exception.Message);
  }

  [Fact(DisplayName = "RenewAsync: it should throw InvalidCredentialsException when session is not found.")]
  public async Task RenewAsync_it_should_throw_InvalidCredentialsException_when_session_is_not_found()
  {
    SessionAggregate session = new(_user, isPersistent: true);
    RenewPayload payload = new()
    {
      RefreshToken = new RefreshToken(session).ToString()
    };
    var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _sessionService.RenewAsync(payload, CancellationToken));
    Assert.StartsWith($"The session '{session.Id}' could not be found.", exception.Message);
  }

  [Fact(DisplayName = "RenewAsync: it should throw InvalidCredentialsException when session is not persistent.")]
  public async Task RenewAsync_it_should_throw_InvalidCredentialsException_when_session_is_not_persistent()
  {
    SessionAggregate session = new(_user, isPersistent: false);
    await _sessionRepository.SaveAsync(session);

    byte[] secret = RandomNumberGenerator.GetBytes(32);
    session.GetType().GetProperty("Secret")?.SetValue(session, secret);
    Assert.NotNull(session.Secret);

    RenewPayload payload = new()
    {
      RefreshToken = new RefreshToken(session).ToString()
    };
    var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _sessionService.RenewAsync(payload, CancellationToken));
    Assert.StartsWith("The specified secret does not match the session.", exception.Message);
  }

  [Fact(DisplayName = "RenewAsync: it should throw InvalidCredentialsException when session secret is not valid.")]
  public async Task RenewAsync_it_should_throw_InvalidCredentialsException_when_session_secret_is_not_valid()
  {
    SessionAggregate session = new(_user, isPersistent: true);
    await _sessionRepository.SaveAsync(session);

    byte[] secret = RandomNumberGenerator.GetBytes(32);
    session.GetType().GetProperty("Secret")?.SetValue(session, secret);
    Assert.NotNull(session.Secret);

    RenewPayload payload = new()
    {
      RefreshToken = new RefreshToken(session).ToString()
    };
    var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _sessionService.RenewAsync(payload, CancellationToken));
    Assert.StartsWith("The specified secret does not match the session.", exception.Message);
  }

  [Fact(DisplayName = "RenewAsync: it should throw SessionIsNotActiveException when session is not active")]
  public async Task RenewAsync_it_should_throw_SessionIsNotActiveException_when_session_is_not_active()
  {
    SessionAggregate session = new(_user, isPersistent: true);
    session.SignOut();
    await _sessionRepository.SaveAsync(session);

    RenewPayload payload = new()
    {
      RefreshToken = new RefreshToken(session).ToString()
    };
    var exception = await Assert.ThrowsAsync<SessionIsNotActiveException>(
      async () => await _sessionService.RenewAsync(payload, CancellationToken));
    Assert.Equal(session.ToString(), exception.Session);
  }

  [Fact(DisplayName = "SearchAsync: it should return the correct search results.")]
  public async Task SearchAsync_it_should_return_the_correct_search_results()
  {
    UserSettings userSettings = _userSettings.Value;
    string tenantId = Guid.NewGuid().ToString();

    UserAggregate user = new(userSettings.UniqueNameSettings, "admin", tenantId)
    {
      Phone = new PhoneNumber("+15149322582", countryCode: "CA", extension: "4232", isVerified: true)
    };
    await _userRepository.SaveAsync(user);

    SessionAggregate session = _user.SignIn(userSettings, isPersistent: true);
    SessionAggregate session1 = user.SignIn(userSettings, isPersistent: true);
    SessionAggregate session2 = user.SignIn(userSettings, isPersistent: true);
    SessionAggregate session3 = user.SignIn(userSettings, isPersistent: true);
    SessionAggregate notPersistent = user.SignIn(userSettings, isPersistent: false);
    SessionAggregate notActive = user.SignIn(userSettings, isPersistent: true);
    notActive.SignOut();
    SessionAggregate[] sessions = new[] { session, session1, session2, session3, notPersistent, notActive };
    await _sessionRepository.SaveAsync(sessions);

    SessionAggregate other = user.SignIn(userSettings, isPersistent: true);
    await _sessionRepository.SaveAsync(session);

    SearchSessionPayload payload = new()
    {
      IsPersistent = true,
      IsActive = true,
      Sort = new[]
      {
        new SessionSortOption((SessionSort)(-1)),
        new SessionSortOption(SessionSort.UpdatedOn, isDescending: true)
      },
      Skip = 1,
      Limit = -10
    };
    payload.Id.Operator = SearchOperator.Or;
    payload.Id.Terms = sessions.Select(session => new SearchTerm(session.Id.Value));
    payload.Search.Terms = new[] { new SearchTerm("%ADMIN%") };
    payload.TenantId.Terms = new[] { new SearchTerm(tenantId) };
    payload.UserId.Operator = (SearchOperator)(-1);
    payload.UserId.Terms = new[] { new SearchTerm(_user.Id.Value) };

    SearchResults<Session> results = await _sessionService.SearchAsync(payload, CancellationToken);
    Assert.Equal(3, results.Total);
    Assert.Equal(2, results.Items.Count());
    Assert.Equal(session2.Id.Value, results.Items.ElementAt(0).Id);
    Assert.Equal(session1.Id.Value, results.Items.ElementAt(1).Id);
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
    Assert.Equal(session.User.Id, session.CreatedBy.Id);
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
    Assert.Equal(session.User.Id, session.CreatedBy.Id);
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
    var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _sessionService.SignInAsync(payload, CancellationToken));
    Assert.StartsWith("The specified password does not match the user.", exception.Message);
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
    var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _sessionService.SignInAsync(payload, CancellationToken));
    Assert.StartsWith("The specified password does not match the user.", exception.Message);
  }

  [Fact(DisplayName = "SignInAsync: it should throw InvalidCredentialsException when user is not found.")]
  public async Task SignInAsync_it_should_throw_InvalidCredentialsException_when_user_is_not_found()
  {
    SignInPayload payload = new()
    {
      TenantId = Guid.Empty.ToString(),
      UniqueName = _user.UniqueName
    };
    var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(
      async () => await _sessionService.SignInAsync(payload, CancellationToken));
    Assert.StartsWith("The specified user could not be found.", exception.Message);
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

  [Fact(DisplayName = "SignOutAsync: it return null when session is not found.")]
  public async Task SignOutAsync_it_return_null_when_session_is_not_found()
  {
    Session? session = await _sessionService.SignOutAsync(Guid.Empty.ToString(), CancellationToken);
    Assert.Null(session);
  }

  [Fact(DisplayName = "SignOutAsync: it should sign out the correct session.")]
  public async Task SignOutAsync_it_should_sign_out_the_correct_session()
  {
    SessionAggregate aggregate = new(_user);
    await _sessionRepository.SaveAsync(aggregate);

    Session? session = await _sessionService.SignOutAsync(aggregate.Id.Value, CancellationToken);
    Assert.NotNull(session);
    Assert.Equal(aggregate.Id.Value, session.Id);
    Assert.False(session.IsActive);
    Assert.Equal(Actor, session.SignedOutBy);
    Assert.NotNull(session.SignedOutOn);
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
    Assert.Equal(session.Id, values[1]);
    Assert.True(secret.IsMatch(Convert.FromBase64String(values[2].FromUriSafeBase64())));
  }
}
