using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Managers;

[Trait(Traits.Category, Categories.Integration)]
public class SessionManagerTests : IntegrationTests, IAsyncLifetime
{
  private readonly string _tenantId = Guid.NewGuid().ToString();

  private readonly IAggregateRepository _aggregateRepository;
  private readonly ISessionManager _sessionManager;

  private readonly SessionAggregate _session;
  private readonly UserAggregate _user;

  public SessionManagerTests()
  {
    _aggregateRepository = ServiceProvider.GetRequiredService<IAggregateRepository>();
    _sessionManager = ServiceProvider.GetRequiredService<ISessionManager>();

    _user = new(UserSettings.UniqueNameSettings, "admin", _tenantId);

    _session = new(_user);
  }

  [Fact(DisplayName = "DeleteAsync: it should delete the session.")]
  public async Task DeleteAsync_it_should_delete_the_session()
  {
    await _sessionManager.DeleteAsync(_session);

    SessionAggregate? session = await _aggregateRepository.LoadAsync<SessionAggregate>(_session.Id, includeDeleted: true);
    Assert.NotNull(session);
    Assert.True(session.IsDeleted);
  }

  [Fact(DisplayName = "SaveAsync: it should save the session.")]
  public async Task SaveAsync_it_should_save_the_session()
  {
    _session.SetCustomAttribute("IpAddress", "::1");
    await _sessionManager.SaveAsync(_session);

    SessionAggregate? session = await _aggregateRepository.LoadAsync<SessionAggregate>(_session.Id);
    Assert.NotNull(session);
    Assert.Equal(_session.CustomAttributes["IpAddress"], session.CustomAttributes["IpAddress"]);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _aggregateRepository.SaveAsync(new AggregateRoot[] { _user, _session });
  }
}
