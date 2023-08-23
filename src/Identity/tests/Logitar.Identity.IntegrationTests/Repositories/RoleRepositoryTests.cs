﻿using Logitar.EventSourcing;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Repositories;

[Trait(Traits.Category, Categories.Integration)]
public class RoleRepositoryTests : IntegrationTests, IAsyncLifetime
{
  private readonly string _tenantId = Guid.NewGuid().ToString();

  private readonly IAggregateRepository _aggregateRepository;
  private readonly IRoleRepository _roleRepository;

  private readonly RoleAggregate _admin;
  private readonly RoleAggregate _guest;
  private readonly RoleAggregate _deleted;
  private readonly RoleAggregate _noTenant;
  private readonly UserAggregate _user;

  public RoleRepositoryTests() : base()
  {
    _aggregateRepository = ServiceProvider.GetRequiredService<IAggregateRepository>();
    _roleRepository = ServiceProvider.GetRequiredService<IRoleRepository>();

    _admin = new(RoleSettings.UniqueNameSettings, "admin", _tenantId)
    {
      DisplayName = "Administrator",
      Description = "This is the administrator role of the current tenant."
    };
    _admin.SetCustomAttribute("write_users", bool.TrueString);

    _guest = new(RoleSettings.UniqueNameSettings, "guest", _tenantId);

    _deleted = new(RoleSettings.UniqueNameSettings, "deleted", _tenantId);
    _deleted.Delete();

    _noTenant = new(RoleSettings.UniqueNameSettings, _admin.UniqueName);

    _user = new(UserSettings.UniqueNameSettings, _admin.UniqueName, _admin.TenantId);
    _user.AddRole(_admin);
    _user.AddRole(_guest);
    _user.AddRole(_deleted);
  }

  [Fact(DisplayName = "LoadAsync: it loads the correct role by unique name.")]
  public async Task LoadAsync_it_loads_the_correct_role_by_unique_name()
  {
    RoleAggregate? role = await _roleRepository.LoadAsync(tenantId: null, _noTenant.UniqueName);
    Assert.NotNull(role);
    Assert.Equal(_noTenant, role);
  }

  [Fact(DisplayName = "LoadAsync: it loads the correct roles by tenant.")]
  public async Task LoadAsync_it_loads_the_correct_roles_by_tenant()
  {
    IEnumerable<RoleAggregate> roles = await _roleRepository.LoadAsync(_tenantId);
    Assert.Equal(2, roles.Count());
    Assert.Contains(roles, role => role.Equals(_admin));
    Assert.Contains(roles, role => role.Equals(_guest));
  }

  [Fact(DisplayName = "LoadAsync: it loads the correct roles by user.")]
  public async Task LoadAsync_it_loads_the_correct_roles_by_user()
  {
    IEnumerable<RoleAggregate> roles = await _roleRepository.LoadAsync(_user);
    Assert.Equal(2, roles.Count());
    Assert.Contains(roles, role => role.Equals(_admin));
    Assert.Contains(roles, role => role.Equals(_guest));
  }

  [Fact(DisplayName = "SaveAsync: it saves the correct role.")]
  public async Task SaveAsync_it_saves_the_correct_role()
  {
    Dictionary<string, RoleEntity> roles = await IdentityContext.Roles.AsNoTracking()
      .ToDictionaryAsync(x => x.AggregateId, x => x);

    IEnumerable<RoleAggregate> aggregates = new[] { _admin, _guest, _deleted, _noTenant };
    foreach (RoleAggregate aggregate in aggregates)
    {
      if (aggregate.IsDeleted)
      {
        Assert.False(roles.ContainsKey(aggregate.Id.Value));
      }
      else
      {
        RoleEntity role = roles[aggregate.Id.Value];

        Assert.Equal(aggregate.CreatedBy.Value, role.CreatedBy);
        AssertEqual(aggregate.CreatedOn, role.CreatedOn);
        Assert.Equal(aggregate.UpdatedBy.Value, role.UpdatedBy);
        AssertEqual(aggregate.UpdatedOn, role.UpdatedOn);
        Assert.Equal(aggregate.Version, role.Version);

        Assert.Equal(aggregate.TenantId, role.TenantId);
        Assert.Equal(aggregate.UniqueName, role.UniqueName);
        Assert.Equal(aggregate.DisplayName, role.DisplayName);
        Assert.Equal(aggregate.Description, role.Description);
        Assert.Equal(aggregate.CustomAttributes, role.CustomAttributes);
      }
    }
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _aggregateRepository.SaveAsync(new AggregateRoot[] { _admin, _guest, _deleted, _noTenant, _user });
  }
}
