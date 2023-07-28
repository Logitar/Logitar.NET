using Logitar.Identity.Core;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Payloads;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.IntegrationTests.Roles;

[Trait(Traits.Category, Categories.Integration)]
public class RoleServiceTests : IntegrationTestingBase
{
  private readonly IApiKeyRepository _apiKeyRepository;
  private readonly IRoleRepository _roleRepository;
  private readonly IRoleService _roleService;
  private readonly IOptions<RoleSettings> _roleSettings;

  private readonly RoleAggregate _role;

  public RoleServiceTests() : base()
  {
    _apiKeyRepository = ServiceProvider.GetRequiredService<IApiKeyRepository>();
    _roleRepository = ServiceProvider.GetRequiredService<IRoleRepository>();
    _roleService = ServiceProvider.GetRequiredService<IRoleService>();
    _roleSettings = ServiceProvider.GetRequiredService<IOptions<RoleSettings>>();

    RoleSettings roleSettings = _roleSettings.Value;
    _role = new(roleSettings.UniqueNameSettings, "admin", tenantId: Guid.NewGuid().ToString());
  }

  [Fact]
  public async Task CreateAsync_it_should_create_the_correct_role()
  {
    CreateRolePayload payload = new()
    {
      TenantId = _role.TenantId,
      UniqueName = $"{_role.UniqueName}2",
      DisplayName = "  Administrator  ",
      Description = "    "
    };
    Role? role = await _roleService.CreateAsync(payload, CancellationToken);
    Assert.NotNull(role);
    Assert.Equal(payload.TenantId, role.TenantId);
    Assert.Equal(payload.UniqueName, role.UniqueName);
    Assert.Equal(payload.DisplayName.Trim(), role.DisplayName);
    Assert.Null(role.Description);
  }

  [Fact]
  public async Task CreateAsync_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    CreateRolePayload payload = new()
    {
      TenantId = _role.TenantId,
      UniqueName = _role.UniqueName
    };
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<RoleAggregate>>(
      async () => await _roleService.CreateAsync(payload, CancellationToken));
    Assert.Equal(payload.TenantId, exception.TenantId);
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact]
  public async Task DeleteAsync_it_should_delete_the_correct_role()
  {
    ApiKeyAggregate apiKey = new("Default", _role.TenantId);
    apiKey.AddRole(_role);
    Assert.Contains(apiKey.Roles, roleId => roleId == _role.Id);
    await _apiKeyRepository.SaveAsync(apiKey);

    Assert.True(await IdentityContext.Roles.AnyAsync(x => x.AggregateId == _role.Id.Value));

    Role? role = await _roleService.DeleteAsync(_role.Id.Value, CancellationToken);
    Assert.NotNull(role);

    Assert.False(await IdentityContext.Roles.AnyAsync(x => x.AggregateId == _role.Id.Value));

    apiKey = (await _apiKeyRepository.LoadAsync(apiKey.Id))!;
    Assert.NotNull(apiKey);
    Assert.DoesNotContain(apiKey.Roles, roleId => roleId == _role.Id);
  }

  [Fact]
  public async Task DeleteAsync_it_should_return_null_when_role_is_not_found()
  {
    Role? role = await _roleService.DeleteAsync(Guid.Empty.ToString(), CancellationToken);
    Assert.Null(role);
  }

  [Fact]
  public async Task ReadAsync_it_should_read_the_correct_role_by_id()
  {
    Role? role = await _roleService.ReadAsync(_role.Id.Value, _role.TenantId, "admin2", CancellationToken);
    Assert.NotNull(role);
    Assert.Equal(_role.Id.Value, role.Id);
  }

  [Fact]
  public async Task ReadAsync_it_should_read_the_correct_role_by_unique_name()
  {
    Role? role = await _roleService.ReadAsync(id: Guid.Empty.ToString(), _role.TenantId, _role.UniqueName, CancellationToken);
    Assert.NotNull(role);
    Assert.Equal(_role.Id.Value, role.Id);
  }

  [Fact]
  public async Task ReadAsync_it_should_return_null_when_no_role_is_a_match()
  {
    Role? role = await _roleService.ReadAsync(id: Guid.Empty.ToString(), _role.TenantId, "admin2", CancellationToken);
    Assert.Null(role);
  }

  [Fact]
  public async Task ReadAsync_it_should_throw_TooManyResultsException_when_many_roles_are_a_match()
  {
    RoleSettings roleSettings = _roleSettings.Value;
    RoleAggregate guest = new(roleSettings.UniqueNameSettings, "guest", _role.TenantId);
    await _roleRepository.SaveAsync(guest);

    var exception = await Assert.ThrowsAsync<TooManyResultsException<Role>>(
      async () => await _roleService.ReadAsync(_role.Id.Value, guest.TenantId, guest.UniqueName, CancellationToken));
    Assert.Equal(2, exception.Actual);
  }

  [Fact]
  public async Task ReplaceAsync_it_should_replace_the_correct_role()
  {
    ReplaceRolePayload payload = new()
    {
      UniqueName = " admin2 ",
      DisplayName = "  Administrator  ",
      Description = "    "
    };
    Role? role = await _roleService.ReplaceAsync(_role.Id.Value, payload, CancellationToken);
    Assert.NotNull(role);
    Assert.Equal(payload.UniqueName.Trim(), role.UniqueName);
    Assert.Equal(payload.DisplayName.Trim(), role.DisplayName);
    Assert.Null(role.Description);
  }

  [Fact]
  public async Task ReplaceAsync_it_should_return_null_when_role_is_not_found()
  {
    ReplaceRolePayload payload = new();
    Role? role = await _roleService.ReplaceAsync(Guid.Empty.ToString(), payload, CancellationToken);
    Assert.Null(role);
  }

  [Fact]
  public async Task ReplaceAsync_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    RoleSettings roleSettings = _roleSettings.Value;
    RoleAggregate guest = new(roleSettings.UniqueNameSettings, "guest", _role.TenantId);
    await _roleRepository.SaveAsync(guest);

    ReplaceRolePayload payload = new()
    {
      UniqueName = _role.UniqueName
    };
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<RoleAggregate>>(
      async () => await _roleService.ReplaceAsync(guest.Id.Value, payload, CancellationToken));
    Assert.Equal(_role.TenantId, exception.TenantId);
    Assert.Equal(_role.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact]
  public async Task SearchAsync_it_should_return_the_correct_roles()
  {
    RoleSettings roleSettings = _roleSettings.Value;
    string tenantId = Guid.NewGuid().ToString();

    RoleAggregate deleteUsers = new(roleSettings.UniqueNameSettings, "delete_users", tenantId);
    RoleAggregate readUsers = new(roleSettings.UniqueNameSettings, "read_users", tenantId);
    RoleAggregate writeUsers = new(roleSettings.UniqueNameSettings, "write_users", tenantId);
    RoleAggregate otherTenant = new(roleSettings.UniqueNameSettings, "manage_users");
    await _roleRepository.SaveAsync(new[] { deleteUsers, readUsers, writeUsers, otherTenant });

    SearchRolePayload payload = new()
    {
      Sort = new[]
      {
        new RoleSortOption((RoleSort)(-1)),
        new RoleSortOption(RoleSort.UniqueName, isDescending: true)
      },
      Skip = 1,
      Limit = -999
    };
    payload.Id.Operator = (SearchOperator)(-1);
    payload.Id.Terms = new[] { new SearchTerm(_role.Id.Value) };
    payload.Search.Terms = new[] { new SearchTerm("%UsEr%") };
    payload.TenantId.Terms = new[] { new SearchTerm(tenantId) };

    SearchResults<Role> roles = await _roleService.SearchAsync(payload, CancellationToken);
    Assert.Equal(3, roles.Total);
    Assert.Equal(2, roles.Items.Count());
    Assert.Equal(readUsers.Id.Value, roles.Items.ElementAt(0).Id);
    Assert.Equal(writeUsers.Id.Value, roles.Items.ElementAt(1).Id);
  }

  [Fact]
  public async Task UpdateAsync_it_should_update_the_correct_role()
  {
    _role.DisplayName = "Administrator";
    await _roleRepository.SaveAsync(_role);

    UpdateRolePayload payload = new()
    {
      UniqueName = " admin2 ",
      DisplayName = new MayBe<string>("    "),
      Description = new MayBe<string>("    ")
    };
    Role? role = await _roleService.UpdateAsync(_role.Id.Value, payload, CancellationToken);
    Assert.NotNull(role);
    Assert.Equal(payload.UniqueName.Trim(), role.UniqueName);
    Assert.Null(role.DisplayName);
    Assert.Null(role.Description);
  }

  [Fact]
  public async Task UpdateAsync_it_should_return_null_when_role_is_not_found()
  {
    UpdateRolePayload payload = new();
    Role? role = await _roleService.UpdateAsync(Guid.Empty.ToString(), payload, CancellationToken);
    Assert.Null(role);
  }

  [Fact]
  public async Task UpdateAsync_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    RoleSettings roleSettings = _roleSettings.Value;
    RoleAggregate guest = new(roleSettings.UniqueNameSettings, "guest", _role.TenantId);
    await _roleRepository.SaveAsync(guest);

    UpdateRolePayload payload = new()
    {
      UniqueName = _role.UniqueName
    };
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<RoleAggregate>>(
      async () => await _roleService.UpdateAsync(guest.Id.Value, payload, CancellationToken));
    Assert.Equal(_role.TenantId, exception.TenantId);
    Assert.Equal(_role.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _roleRepository.SaveAsync(_role);
  }
}
