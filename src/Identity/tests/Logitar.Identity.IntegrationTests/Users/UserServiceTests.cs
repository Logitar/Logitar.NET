﻿using Logitar.Identity.Core;
using Logitar.Identity.Core.Users;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.IntegrationTests.Users;

[Trait(Traits.Category, Categories.Integration)]
public class UserServiceTests : IntegrationTestingBase
{
  private readonly IUserService _userService;

  private readonly UserAggregate _user;

  public UserServiceTests() : base()
  {
    _userService = ServiceProvider.GetRequiredService<IUserService>();

    IOptions<UserSettings> userSettings = ServiceProvider.GetRequiredService<IOptions<UserSettings>>();
    _user = new(userSettings.Value.UniqueNameSettings, uniqueName: "admin", tenantId: Guid.NewGuid().ToString())
    {
      Email = new(Faker.Person.Email, isVerified: true)
    };
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct user.")]
  public async Task CreateAsync_it_should_create_the_correct_user()
  {
    Assert.NotNull(_user.Email);
    string[] emailParts = _user.Email.Address.Split('@');
    emailParts[0] = $"{emailParts[0]}2";
    string emailAddress = string.Join('@', emailParts);
    CreateUserPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = $"{_user.UniqueName}2",
      Password = "Test123!",
      IsDisabled = true,
      Email = new CreateEmailPayload
      {
        Address = emailAddress,
        IsVerified = _user.Email.IsVerified
      },
      FirstName = "Charles",
      LastName = "Raymond",
      Locale = "fr-CA"
    };
    User user = await _userService.CreateAsync(payload, CancellationToken);
    Assert.NotNull(user.Email);
    Assert.Equal(payload.TenantId, user.TenantId);
    Assert.Equal(payload.UniqueName, user.UniqueName);
    Assert.Equal(payload.Password != null, user.HasPassword);
    Assert.Equal(Actor, user.PasswordChangedBy);
    Assert.Equal(payload.IsDisabled, user.IsDisabled);
    Assert.Equal(Actor, user.DisabledBy);
    Assert.Equal(emailAddress, user.Email.Address);
    Assert.Equal(payload.Email.IsVerified, user.Email.IsVerified);
    Assert.Equal(payload.Email.IsVerified, user.IsConfirmed);
    Assert.Equal(payload.FirstName, user.FirstName);
    Assert.Equal(payload.LastName, user.LastName);
    Assert.Equal(PersonHelper.BuildFullName(payload.FirstName, payload.LastName), user.FullName);
    Assert.Equal(payload.Locale, user.Locale);
    Assert.NotNull(user.PasswordChangedOn);
    Assert.NotNull(user.DisabledOn);
  }

  [Fact(DisplayName = "CreateAsync: it should throw EmailAddressAlreadyUsedException when email address is already used.")]
  public async Task CreateAsync_it_should_throw_EmailAddressAlreadyUsedException_when_email_address_is_already_used()
  {
    Assert.NotNull(_user.Email);
    CreateUserPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = $"{_user.UniqueName}2",
      Email = new CreateEmailPayload
      {
        Address = _user.Email.Address,
        IsVerified = _user.Email.IsVerified
      }
    };
    var exception = await Assert.ThrowsAsync<EmailAddressAlreadyUsedException>(
      async () => await _userService.CreateAsync(payload, CancellationToken));
    Assert.Equal(payload.TenantId, exception.TenantId);
    Assert.Equal(payload.Email.Address, exception.EmailAddress);
    Assert.Equal("Email", exception.PropertyName);
  }

  [Fact(DisplayName = "CreateAsync: it should throw UniqueNameAlreadyUsedException when unique name is already used.")]
  public async Task CreateAsync_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    CreateUserPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = _user.UniqueName
    };
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<UserAggregate>>(
      async () => await _userService.CreateAsync(payload, CancellationToken));
    Assert.Equal(payload.TenantId, exception.TenantId);
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact(DisplayName = "UpdateAsync: it should return null when user is not found.")]
  public async Task UpdateAsync_it_should_return_null_when_user_is_not_found()
  {
    UpdateUserPayload payload = new();
    User? user = await _userService.UpdateAsync(Guid.Empty.ToString(), payload, CancellationToken);
    Assert.Null(user);
  }

  [Fact(DisplayName = "UpdateAsync: it should throw EmailAddressAlreadyUsedException when unique name is already used.")]
  public async Task UpdateAsync_it_should_throw_EmailAddressAlreadyUsedException_when_unique_name_is_already_used()
  {
    CreateUserPayload createPayload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = "test"
    };
    User user = await _userService.CreateAsync(createPayload, CancellationToken);

    Assert.NotNull(_user.Email);
    UpdateUserPayload updatePayload = new()
    {
      Email = new MayBe<UpdateEmailPayload>(new UpdateEmailPayload
      {
        Address = _user.Email.Address,
        IsVerified = _user.Email.IsVerified
      })
    };
    Assert.NotNull(updatePayload.Email.Value);
    var exception = await Assert.ThrowsAsync<EmailAddressAlreadyUsedException>(
      async () => await _userService.UpdateAsync(user.Id, updatePayload, CancellationToken));
    Assert.Equal(user.TenantId, exception.TenantId);
    Assert.Equal(updatePayload.Email.Value.Address, exception.EmailAddress);
    Assert.Equal("Email", exception.PropertyName);
  }

  [Fact(DisplayName = "UpdateAsync: it should throw UniqueNameAlreadyUsedException when unique name is already used.")]
  public async Task UpdateAsync_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    CreateUserPayload createPayload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = "test"
    };
    User user = await _userService.CreateAsync(createPayload, CancellationToken);

    UpdateUserPayload updatePayload = new()
    {
      UniqueName = _user.UniqueName.ToUpper()
    };
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<UserAggregate>>(
      async () => await _userService.UpdateAsync(user.Id, updatePayload, CancellationToken));
    Assert.Equal(user.TenantId, exception.TenantId);
    Assert.Equal(updatePayload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  [Fact(DisplayName = "UpdateAsync: it should update the correct user.")]
  public async Task UpdateAsync_it_should_update_the_correct_user()
  {
    Assert.False(_user.HasPassword);
    Assert.NotNull(_user.Email);
    UpdateUserPayload payload = new()
    {
      UniqueName = $"{_user.UniqueName}2",
      Password = "Test123!",
      IsDisabled = true,
      Email = new MayBe<UpdateEmailPayload>(new UpdateEmailPayload
      {
        Address = Faker.Internet.Email()
      }),
      FirstName = new MayBe<string>(Faker.Person.FirstName),
      LastName = new MayBe<string>(Faker.Person.LastName),
      Locale = new MayBe<string>("en-US")
    };
    Assert.NotNull(payload.Email.Value);
    User? user = await _userService.UpdateAsync(_user.Id.Value, payload, CancellationToken);
    Assert.NotNull(user);
    Assert.NotNull(user.Email);
    Assert.Equal(payload.UniqueName, user.UniqueName);
    Assert.Equal(payload.IsDisabled, user.IsDisabled);
    Assert.Equal(payload.Email.Value.Address, user.Email.Address);
    Assert.Equal(payload.Email.Value.IsVerified ?? _user.Email.IsVerified, user.Email.IsVerified);
    Assert.Equal(payload.FirstName.Value, user.FirstName);
    Assert.Equal(payload.LastName.Value, user.LastName);
    Assert.Equal(payload.Locale.Value, user.Locale);
    Assert.True(user.HasPassword);
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    IUserRepository userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
    await userRepository.SaveAsync(_user);
  }
}
