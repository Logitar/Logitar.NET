using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Repositories;

[Trait(Traits.Category, Categories.Integration)]
public class SessionRepositoryTests : IntegrationTestBase, IAsyncLifetime
{
  private readonly string _tenantId = Guid.NewGuid().ToString();

  private readonly IAggregateRepository _aggregateRepository;
  private readonly IPasswordService _passwordService;
  private readonly ISessionRepository _sessionRepository;
  private readonly IOptions<UserSettings> _userSettings;

  private readonly byte[] _secretBytes;
  private readonly UserAggregate _user;
  private readonly UserAggregate _noTenantUser;
  private readonly SessionAggregate _session;
  private readonly SessionAggregate _persistent;
  private readonly SessionAggregate _signedOut;
  private readonly SessionAggregate _deleted;
  private readonly SessionAggregate _noTenantSession;

  public SessionRepositoryTests() : base()
  {
    _aggregateRepository = ServiceProvider.GetRequiredService<IAggregateRepository>();
    _passwordService = ServiceProvider.GetRequiredService<IPasswordService>();
    _sessionRepository = ServiceProvider.GetRequiredService<ISessionRepository>();
    _userSettings = ServiceProvider.GetRequiredService<IOptions<UserSettings>>();

    UserSettings userSettings = _userSettings.Value;
    _user = new(userSettings.UniqueNameSettings, "admin", _tenantId);
    _noTenantUser = new(userSettings.UniqueNameSettings, _user.UniqueName, tenantId: null);

    _session = new(_user, secret: null);

    Password secret = _passwordService.Generate(256 / 8, out _secretBytes);
    _persistent = new(_user, secret);
    _persistent.SetCustomAttribute("IpAddress", "::1");

    _signedOut = new(_user, secret: null);
    _signedOut.SignOut();

    _deleted = new(_user, secret: null);
    _deleted.Delete();

    _noTenantSession = new(_noTenantUser, secret: null);
  }

  [Fact(DisplayName = "LoadAsync: it loads the correct active sessions by user.")]
  public async Task LoadAsync_it_loads_the_correct_active_sessions_by_user()
  {
    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(_user, isActive: true);
    Assert.Equal(2, sessions.Count());
    Assert.Contains(sessions, session => session.Equals(_session));
    Assert.Contains(sessions, session => session.Equals(_persistent));
  }

  [Fact(DisplayName = "LoadAsync: it loads the correct sessions by tenant.")]
  public async Task LoadAsync_it_loads_the_correct_sessions_by_tenant()
  {
    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(_tenantId);
    Assert.Equal(3, sessions.Count());
    Assert.Contains(sessions, session => session.Equals(_session));
    Assert.Contains(sessions, session => session.Equals(_persistent));
    Assert.Contains(sessions, session => session.Equals(_signedOut));
  }

  [Fact(DisplayName = "LoadAsync: it loads the correct sessions by user.")]
  public async Task LoadAsync_it_loads_the_correct_sessions_by_user()
  {
    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(_user);
    Assert.Equal(3, sessions.Count());
    Assert.Contains(sessions, session => session.Equals(_session));
    Assert.Contains(sessions, session => session.Equals(_persistent));
    Assert.Contains(sessions, session => session.Equals(_signedOut));
  }

  [Fact(DisplayName = "LoadAsync: it loads the correct unactive sessions by user.")]
  public async Task LoadAsync_it_loads_the_correct_unactive_sessions_by_user()
  {
    IEnumerable<SessionAggregate> sessions = await _sessionRepository.LoadAsync(_user, isActive: false);
    SessionAggregate session = Assert.Single(sessions);
    Assert.Equal(_signedOut, session);
  }

  [Fact(DisplayName = "SaveAsync: it saves the correct session.")]
  public async Task SaveAsync_it_saves_the_correct_session()
  {
    Password newSecret = _passwordService.Generate(256 / 8, out byte[] secretBytes);
    _persistent.Renew(Convert.ToBase64String(_secretBytes), newSecret);
    await _aggregateRepository.SaveAsync(_persistent);

    Dictionary<string, SessionEntity> sessions = await IdentityContext.Sessions.AsNoTracking()
      .Include(x => x.User)
      .ToDictionaryAsync(x => x.AggregateId, x => x);

    IEnumerable<SessionAggregate> aggregates = new[] { _session, _persistent, _signedOut, _deleted, _noTenantSession };
    foreach (SessionAggregate aggregate in aggregates)
    {
      if (aggregate.IsDeleted)
      {
        Assert.False(sessions.ContainsKey(aggregate.Id.Value));
      }
      else
      {
        SessionEntity session = sessions[aggregate.Id.Value];

        Assert.Equal(aggregate.CreatedBy.Value, session.CreatedBy);
        AssertEqual(aggregate.CreatedOn, session.CreatedOn);
        Assert.Equal(aggregate.UpdatedBy.Value, session.UpdatedBy);
        AssertEqual(aggregate.UpdatedOn, session.UpdatedOn);
        Assert.Equal(aggregate.Version, session.Version);

        Assert.Equal(aggregate.UserId.Value, session.User?.AggregateId);
        Assert.Equal(aggregate.IsPersistent, session.IsPersistent);
        Assert.Equal(aggregate.IsActive, session.IsActive);
        Assert.Equal(aggregate.CustomAttributes, session.CustomAttributes);
      }
    }

    SessionEntity persistent = sessions[_persistent.Id.Value];
    Assert.NotNull(persistent.Secret);
    Password password = _passwordService.Decode(persistent.Secret);
    Assert.True(password.IsMatch(Convert.ToBase64String(secretBytes)));
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _aggregateRepository.SaveAsync(new AggregateRoot[] { _user, _noTenantUser, _session, _persistent, _signedOut, _deleted, _noTenantSession });
  }
}
