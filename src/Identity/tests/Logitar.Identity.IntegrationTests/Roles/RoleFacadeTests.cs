using Logitar.Identity.Core;
using Logitar.Identity.Core.ApiKeys;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Payloads;
using Logitar.Identity.Core.Roles;
using Logitar.Identity.Core.Roles.Models;
using Logitar.Identity.Core.Roles.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.ApiKeys;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.IntegrationTests.Roles;

[Trait(Traits.Category, Categories.Integration)]
public class RoleFacadeTests : IntegrationTestingBase
{
  private readonly IApiKeyRepository _apiKeyRepository;
  private readonly IPasswordHelper _passwordHelper;
  private readonly IRoleFacade _roleFacade;
  private readonly IRoleRepository _roleRepository;
  private readonly IOptions<RoleSettings> _roleSettings;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  private readonly RoleAggregate _role;

  public RoleFacadeTests() : base()
  {
    _apiKeyRepository = ServiceProvider.GetRequiredService<IApiKeyRepository>();
    _passwordHelper = ServiceProvider.GetRequiredService<IPasswordHelper>();
    _roleFacade = ServiceProvider.GetRequiredService<IRoleFacade>();
    _roleRepository = ServiceProvider.GetRequiredService<IRoleRepository>();
    _roleSettings = ServiceProvider.GetRequiredService<IOptions<RoleSettings>>();
    _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
    _userSettings = ServiceProvider.GetRequiredService<IOptions<UserSettings>>();

    RoleSettings roleSettings = _roleSettings.Value;
    _role = new(roleSettings.UniqueNameSettings, "admin", tenantId: Guid.NewGuid().ToString());
    _role.SetCustomAttribute("ReadUsers", bool.TrueString);
    _role.SetCustomAttribute("WriteUsers", bool.FalseString);
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct role.")]
  public async Task CreateAsync_it_should_create_the_correct_role()
  {
    CreateRolePayload payload = new()
    {
      TenantId = _role.TenantId,
      UniqueName = $"{_role.UniqueName}2",
      DisplayName = "  Administrator  ",
      Description = "    ",
      CustomAttributes = new[]
      {
        new CustomAttribute("ReadUsers", bool.TrueString),
        new CustomAttribute(" WriteUsers   ", $"   {bool.FalseString} ")
      }
    };
    Role? role = await _roleFacade.CreateAsync(payload, CancellationToken);
    Assert.NotNull(role);
    Assert.Equal(payload.TenantId, role.TenantId);
    Assert.Equal(payload.UniqueName, role.UniqueName);
    Assert.Equal(payload.DisplayName.Trim(), role.DisplayName);
    Assert.Null(role.Description);

    Assert.Equal(payload.CustomAttributes.Count(), role.CustomAttributes.Count());
    foreach (CustomAttribute customAttribute in payload.CustomAttributes)
    {
      Assert.Contains(role.CustomAttributes, c => c.Key == customAttribute.Key.Trim() && c.Value == customAttribute.Value.Trim());
    }
  }

  [Fact(DisplayName = "CreateAsync: it should throw UniqueNameAlreadyUsedException when unique name is already used.")]
  public async Task CreateAsync_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    CreateRolePayload payload = new()
    {
      TenantId = _role.TenantId,
      UniqueName = _role.UniqueName
    };
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<RoleAggregate>>(
      async () => await _roleFacade.CreateAsync(payload, CancellationToken));
    Assert.Equal(payload.TenantId, exception.TenantId);
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact(DisplayName = "DeleteAsync: it should delete the correct role.")]
  public async Task DeleteAsync_it_should_delete_the_correct_role()
  {
    Password secret = _passwordHelper.Generate(ApiKeyAggregate.SecretLength, out _);
    ApiKeyAggregate apiKey = new(secret, "Default", _role.TenantId);
    apiKey.AddRole(_role);
    Assert.Contains(apiKey.Roles, roleId => roleId == _role.Id);
    await _apiKeyRepository.SaveAsync(apiKey);

    UserAggregate user = new(_userSettings.Value.UniqueNameSettings, "admin", _role.TenantId);
    user.AddRole(_role);
    Assert.Contains(user.Roles, roleId => roleId == _role.Id);
    await _userRepository.SaveAsync(user);

    Assert.True(await IdentityContext.Roles.AnyAsync(x => x.AggregateId == _role.Id.Value));

    Role? role = await _roleFacade.DeleteAsync(_role.Id.Value, CancellationToken);
    Assert.NotNull(role);

    Assert.False(await IdentityContext.Roles.AnyAsync(x => x.AggregateId == _role.Id.Value));

    apiKey = (await _apiKeyRepository.LoadAsync(apiKey.Id))!;
    Assert.NotNull(apiKey);
    Assert.DoesNotContain(apiKey.Roles, roleId => roleId == _role.Id);

    user = (await _userRepository.LoadAsync(user.Id))!;
    Assert.NotNull(user);
    Assert.DoesNotContain(user.Roles, roleId => roleId == _role.Id);
  }

  [Fact(DisplayName = "DeleteAsync: it should return null when role is not found.")]
  public async Task DeleteAsync_it_should_return_null_when_role_is_not_found()
  {
    Role? role = await _roleFacade.DeleteAsync(Guid.Empty.ToString(), CancellationToken);
    Assert.Null(role);
  }

  [Fact(DisplayName = "ReadAsync: it should read the correct role by ID.")]
  public async Task ReadAsync_it_should_read_the_correct_role_by_Id()
  {
    Role? role = await _roleFacade.ReadAsync(_role.Id.Value, _role.TenantId, "admin2", CancellationToken);
    Assert.NotNull(role);
    Assert.Equal(_role.Id.Value, role.Id);
  }

  [Fact(DisplayName = "ReadAsync: it should read the correct role by unique name.")]
  public async Task ReadAsync_it_should_read_the_correct_role_by_unique_name()
  {
    Role? role = await _roleFacade.ReadAsync(id: Guid.Empty.ToString(), _role.TenantId, _role.UniqueName, CancellationToken);
    Assert.NotNull(role);
    Assert.Equal(_role.Id.Value, role.Id);
  }

  [Fact(DisplayName = "ReadAsync: it should return null when no role is a match.")]
  public async Task ReadAsync_it_should_return_null_when_no_role_is_a_match()
  {
    Role? role = await _roleFacade.ReadAsync(id: Guid.Empty.ToString(), _role.TenantId, "admin2", CancellationToken);
    Assert.Null(role);
  }

  [Fact(DisplayName = "ReadAsync: it should throw TooManyResultsException when many roles are a match.")]
  public async Task ReadAsync_it_should_throw_TooManyResultsException_when_many_roles_are_a_match()
  {
    RoleSettings roleSettings = _roleSettings.Value;
    RoleAggregate guest = new(roleSettings.UniqueNameSettings, "guest", _role.TenantId);
    await _roleRepository.SaveAsync(guest);

    var exception = await Assert.ThrowsAsync<TooManyResultsException<Role>>(
      async () => await _roleFacade.ReadAsync(_role.Id.Value, guest.TenantId, guest.UniqueName, CancellationToken));
    Assert.Equal(2, exception.Actual);
  }

  [Fact(DisplayName = "ReplaceAsync: it should replace the correct role.")]
  public async Task ReplaceAsync_it_should_replace_the_correct_role()
  {
    ReplaceRolePayload payload = new()
    {
      UniqueName = " admin2 ",
      DisplayName = "  Administrator  ",
      Description = "    ",
      CustomAttributes = new[]
      {
        new CustomAttribute("WriteUsers  ", $"  {bool.TrueString}"),
        new CustomAttribute("ViewUsers", bool.FalseString)
      }
    };
    Role? role = await _roleFacade.ReplaceAsync(_role.Id.Value, payload, CancellationToken);
    Assert.NotNull(role);
    Assert.Equal(payload.UniqueName.Trim(), role.UniqueName);
    Assert.Equal(payload.DisplayName.Trim(), role.DisplayName);
    Assert.Null(role.Description);

    Assert.Equal(payload.CustomAttributes.Count(), role.CustomAttributes.Count());
    foreach (CustomAttribute customAttribute in payload.CustomAttributes)
    {
      Assert.Contains(role.CustomAttributes, c => c.Key == customAttribute.Key.Trim() && c.Value == customAttribute.Value.Trim());
    }
  }

  [Fact(DisplayName = "ReplaceAsync: it should return null when role is not found.")]
  public async Task ReplaceAsync_it_should_return_null_when_role_is_not_found()
  {
    ReplaceRolePayload payload = new();
    Role? role = await _roleFacade.ReplaceAsync(Guid.Empty.ToString(), payload, CancellationToken);
    Assert.Null(role);
  }

  [Fact(DisplayName = "ReplaceAsync: it should throw UniqueNameAlreadyUsedException when unique name is already used.")]
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
      async () => await _roleFacade.ReplaceAsync(guest.Id.Value, payload, CancellationToken));
    Assert.Equal(_role.TenantId, exception.TenantId);
    Assert.Equal(_role.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact(DisplayName = "SearchAsync: it should return the correct roles.")]
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

    SearchResults<Role> roles = await _roleFacade.SearchAsync(payload, CancellationToken);
    Assert.Equal(3, roles.Total);
    Assert.Equal(2, roles.Items.Count());
    Assert.Equal(readUsers.Id.Value, roles.Items.ElementAt(0).Id);
    Assert.Equal(writeUsers.Id.Value, roles.Items.ElementAt(1).Id);
  }

  [Fact(DisplayName = "UpdateAsync: it should return null when role is not found.")]
  public async Task UpdateAsync_it_should_return_null_when_role_is_not_found()
  {
    UpdateRolePayload payload = new();
    Role? role = await _roleFacade.UpdateAsync(Guid.Empty.ToString(), payload, CancellationToken);
    Assert.Null(role);
  }

  [Fact(DisplayName = "UpdateAsync: it should throw UniqueNameAlreadyUsedException when unique name is already used.")]
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
      async () => await _roleFacade.UpdateAsync(guest.Id.Value, payload, CancellationToken));
    Assert.Equal(_role.TenantId, exception.TenantId);
    Assert.Equal(_role.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact(DisplayName = "UpdateAsync: it should update the correct role.")]
  public async Task UpdateAsync_it_should_update_the_correct_role()
  {
    _role.DisplayName = "Administrator";
    await _roleRepository.SaveAsync(_role);

    UpdateRolePayload payload = new()
    {
      UniqueName = " admin2 ",
      DisplayName = new MayBe<string>("    "),
      Description = new MayBe<string>("    "),
      CustomAttributes = new[]
      {
        new CustomAttributeModification("ReadUsers", bool.FalseString),
        new CustomAttributeModification(" WriteUsers ", null)
      }
    };
    Role? role = await _roleFacade.UpdateAsync(_role.Id.Value, payload, CancellationToken);
    Assert.NotNull(role);
    Assert.Equal(payload.UniqueName.Trim(), role.UniqueName);
    Assert.Null(role.DisplayName);
    Assert.Null(role.Description);

    CustomAttribute customAttribute = role.CustomAttributes.Single();
    Assert.Equal("ReadUsers", customAttribute.Key);
    Assert.Equal(bool.FalseString, customAttribute.Value);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _roleRepository.SaveAsync(_role);
  }
}
