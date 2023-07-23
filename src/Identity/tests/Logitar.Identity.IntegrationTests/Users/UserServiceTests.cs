using Logitar.Identity.Core;
using Logitar.Identity.Core.Users;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.IntegrationTests.Users;

[Trait(Traits.Category, Categories.Integration)]
public class UserServiceTests : IntegrationTestingBase
{
  private readonly IUserService _userService;

  public UserServiceTests() : base()
  {
    _userService = ServiceProvider.GetRequiredService<IUserService>();
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct user.")]
  public async Task CreateAsync_it_should_create_the_correctUser()
  {
    Assert.NotNull(User.Email);
    string[] emailParts = User.Email.Address.Split('@');
    emailParts[0] = $"{emailParts[0]}2";
    string emailAddress = string.Join('@', emailParts);
    CreateUserPayload payload = new()
    {
      TenantId = User.TenantId,
      UniqueName = $"{User.UniqueName}2",
      Password = "Test123!",
      IsDisabled = true,
      Email = new CreateEmailPayload
      {
        Address = emailAddress,
        IsVerified = User.Email.IsVerified
      },
      FirstName = "Charles",
      LastName = "Raymond",
      Locale = "fr-CA"
    };
    User user = await _userService.CreateAsync(payload, CancellationToken);
    Assert.Equal(payload.TenantId, user.TenantId);
    Assert.Equal(payload.UniqueName, user.UniqueName);
    Assert.True(user.HasPassword);
    Assert.Equal(Actor, user.PasswordChangedBy);
    Assert.NotNull(user.PasswordChangedOn);
    Assert.True(user.IsDisabled);
    Assert.Equal(Actor, user.DisabledBy);
    Assert.NotNull(user.DisabledOn);
    Assert.Equal(emailAddress, user.Email?.Address);
    Assert.True(user.Email?.IsVerified);
    Assert.True(user.IsConfirmed);
    Assert.Equal(payload.FirstName, user.FirstName);
    Assert.Equal(payload.LastName, user.LastName);
    Assert.Equal(PersonHelper.BuildFullName(payload.FirstName, payload.LastName), user.FullName);
    Assert.Equal(payload.Locale, user.Locale);
  }

  [Fact(DisplayName = "CreateAsync: it should throw EmailAddressAlreadyUsedException when email address is already used.")]
  public async Task CreateAsync_it_should_throw_EmailAddressAlreadyUsedException_when_email_address_is_already_used()
  {
    Assert.NotNull(User.Email);
    CreateUserPayload payload = new()
    {
      TenantId = User.TenantId,
      UniqueName = $"{User.UniqueName}2",
      Email = new CreateEmailPayload
      {
        Address = User.Email.Address,
        IsVerified = User.Email.IsVerified
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
      TenantId = User.TenantId,
      UniqueName = User.UniqueName
    };
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<UserAggregate>>(
      async () => await _userService.CreateAsync(payload, CancellationToken));
    Assert.Equal(payload.TenantId, exception.TenantId);
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }
}
