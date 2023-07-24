using Logitar.Identity.Core;
using Logitar.Identity.Core.Sessions;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
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

    Assert.True(session.IsActive);
    Assert.Null(session.SignedOutBy);
    Assert.Null(session.SignedOutOn);

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

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _userRepository.SaveAsync(_user);
  }
}
