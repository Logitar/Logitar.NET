using Logitar.EventSourcing;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Managers;

[Trait(Traits.Category, Categories.Integration)]
public class RoleManagerTests : IntegrationTestBase, IAsyncLifetime
{
  private readonly string _tenantId = Guid.NewGuid().ToString();

  private readonly IAggregateRepository _aggregateRepository;
  private readonly IRoleManager _roleManager;
  private readonly IOptions<RoleSettings> _roleSettings;
  private readonly IOptions<UserSettings> _userSettings;

  private readonly RoleAggregate _role;
  private readonly UserAggregate _user;

  public RoleManagerTests()
  {
    _aggregateRepository = ServiceProvider.GetRequiredService<IAggregateRepository>();
    _roleManager = ServiceProvider.GetRequiredService<IRoleManager>();
    _roleSettings = ServiceProvider.GetRequiredService<IOptions<RoleSettings>>();
    _userSettings = ServiceProvider.GetRequiredService<IOptions<UserSettings>>();

    _role = new(_roleSettings.Value.UniqueNameSettings, "admin", _tenantId);

    _user = new(_userSettings.Value.UniqueNameSettings, "admin", _tenantId);
    _user.AddRole(_role);
  }

  [Fact(DisplayName = "DeleteAsync: it should delete the role and its associations.")]
  public async Task DeleteAsync_it_should_delete_the_role_and_its_associations()
  {
    await _roleManager.DeleteAsync(_role);

    RoleAggregate? role = await _aggregateRepository.LoadAsync<RoleAggregate>(_role.Id, includeDeleted: true);
    Assert.NotNull(role);
    Assert.True(role.IsDeleted);

    UserAggregate? user = await _aggregateRepository.LoadAsync<UserAggregate>(_user.Id);
    Assert.NotNull(user);
    Assert.Empty(user.Roles);
  }

  [Fact(DisplayName = "SaveAsync: it should save the role.")]
  public async Task SaveAsync_it_should_save_the_role()
  {
    _role.DisplayName = "Administrator";
    await _roleManager.SaveAsync(_role);

    RoleAggregate? role = await _aggregateRepository.LoadAsync<RoleAggregate>(_role.Id);
    Assert.NotNull(role);
    Assert.Equal(_role.DisplayName, role.DisplayName);
  }

  [Fact(DisplayName = "SaveAsync: it should throw UniqueNameAlreadyUsedException when unique name is already used.")]
  public async Task SaveAsync_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    RoleAggregate role = new(_roleSettings.Value.UniqueNameSettings, _role.UniqueName, _role.TenantId);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<RoleAggregate>>(
      async () => await _roleManager.SaveAsync(role)
    );
    Assert.Equal(role.TenantId, exception.TenantId);
    Assert.Equal(role.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _aggregateRepository.SaveAsync(new AggregateRoot[] { _role, _user });
  }
}
