﻿using Bogus;
using Logitar.EventSourcing;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.Managers;

[Trait(Traits.Category, Categories.Integration)]
public class UserManagerTests : IntegrationTestBase, IAsyncLifetime
{
  private readonly Faker _faker = new();
  private readonly string _tenantId = Guid.NewGuid().ToString();

  private readonly IAggregateRepository _aggregateRepository;
  private readonly IUserManager _userManager;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  private readonly SessionAggregate _session;
  private readonly UserAggregate _user;

  public UserManagerTests()
  {
    _aggregateRepository = ServiceProvider.GetRequiredService<IAggregateRepository>();
    _userManager = ServiceProvider.GetRequiredService<IUserManager>();
    _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
    _userSettings = ServiceProvider.GetRequiredService<IOptions<UserSettings>>();

    _user = new(_userSettings.Value.UniqueNameSettings, "admin", _tenantId)
    {
      Email = new EmailAddress(_faker.Person.Email)
    };

    _session = new(_user);
  }

  [Fact(DisplayName = "DeleteAsync: it should delete the user and its sessions.")]
  public async Task DeleteAsync_it_should_delete_the_user_and_its_sessions()
  {
    await _userManager.DeleteAsync(_user);

    UserAggregate? user = await _aggregateRepository.LoadAsync<UserAggregate>(_user.Id, includeDeleted: true);
    Assert.NotNull(user);
    Assert.True(user.IsDeleted);

    SessionAggregate? session = await _aggregateRepository.LoadAsync<SessionAggregate>(_session.Id, includeDeleted: true);
    Assert.NotNull(session);
    Assert.True(session.IsDeleted);
  }

  [Fact(DisplayName = "SaveAsync: it should save the user when email unicity is not required.")]
  public async Task SaveAsync_it_should_save_the_user_when_email_unicity_is_not_required()
  {
    UserSettings userSettings = _userSettings.Value;
    userSettings.RequireUniqueEmail = false;

    Assert.NotNull(_user.Email);
    UserAggregate user = new(userSettings.UniqueNameSettings, $"{_user.UniqueName}2", _user.TenantId)
    {
      Email = new EmailAddress(_user.Email.Address)
    };
    await _userManager.SaveAsync(user);

    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(_user.TenantId, _user.Email);
    Assert.Equal(2, users.Count());
    Assert.Contains(users, u => u.Equals(_user));
    Assert.Contains(users, u => u.Equals(user));
  }

  [Fact(DisplayName = "SaveAsync: it should save the user.")]
  public async Task SaveAsync_it_should_save_the_user()
  {
    _user.FirstName = _faker.Person.FirstName;
    _user.LastName = _faker.Person.LastName;
    _user.Birthdate = _faker.Person.DateOfBirth;
    _user.Gender = new Gender(_faker.Person.Gender.ToString());
    await _userManager.SaveAsync(_user);

    UserAggregate? user = await _aggregateRepository.LoadAsync<UserAggregate>(_user.Id);
    Assert.NotNull(user);
    Assert.Equal(_user.FirstName, user.FirstName);
    Assert.Equal(_user.LastName, user.LastName);
    Assert.Equal(_user.Birthdate, user.Birthdate);
    Assert.Equal(_user.Gender, user.Gender);
  }

  [Fact(DisplayName = "SaveAsync: it should throw EmailAddressAlreadyUsedException when email address is already used.")]
  public async Task SaveAsync_it_should_throw_EmailAddressAlreadyUsedException_when_email_address_is_already_used()
  {
    Assert.NotNull(_user.Email);
    UserAggregate user = new(_userSettings.Value.UniqueNameSettings, $"{_user.UniqueName}2", _user.TenantId)
    {
      Email = new EmailAddress(_user.Email.Address)
    };
    var exception = await Assert.ThrowsAsync<EmailAddressAlreadyUsedException>(
      async () => await _userManager.SaveAsync(user)
    );
    Assert.Equal(user.TenantId, exception.TenantId);
    Assert.Equal(user.Email.Address, exception.EmailAddress);
    Assert.Equal("Email", exception.PropertyName);
  }

  [Fact(DisplayName = "SaveAsync: it should throw UniqueNameAlreadyUsedException when unique name is already used.")]
  public async Task SaveAsync_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    UserAggregate user = new(_userSettings.Value.UniqueNameSettings, _user.UniqueName, _user.TenantId);
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<UserAggregate>>(
      async () => await _userManager.SaveAsync(user)
    );
    Assert.Equal(user.TenantId, exception.TenantId);
    Assert.Equal(user.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _aggregateRepository.SaveAsync(new AggregateRoot[] { _user, _session });
  }
}
