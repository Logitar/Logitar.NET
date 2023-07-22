using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Users;
using Logitar.Identity.Core.Users.Models;
using Logitar.Identity.Core.Users.Payloads;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Logitar.Identity.IntegrationTests.Users;

public class UserServiceTests
{
  private static readonly Actor _actor = new();
  private static readonly CancellationToken _cancellationToken = default;

  private static readonly IServiceProvider _serviceProvider;

  private readonly EventContext _eventContext;
  private readonly IdentityContext _identityContext;

  private readonly IUserRepository _userRepository;
  private readonly IUserService _userService;
  private readonly IOptions<UserSettings> _userSettings;

  private readonly UserAggregate _user;

  static UserServiceTests()
  {
    IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json")
      .Build();

    string connectionString = configuration.GetValue<string>("SQLCONNSTR_IntegrationTests") ?? string.Empty;

    _serviceProvider = new ServiceCollection()
      .AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString)
      .AddSingleton<ICurrentActor>(new CurrentActorMock(_actor))
      .BuildServiceProvider();
  }

  public UserServiceTests()
  {
    _eventContext = _serviceProvider.GetRequiredService<EventContext>();
    _identityContext = _serviceProvider.GetRequiredService<IdentityContext>();

    _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
    _userService = _serviceProvider.GetRequiredService<IUserService>();
    _userSettings = _serviceProvider.GetRequiredService<IOptions<UserSettings>>();

    UserSettings userSettings = _userSettings.Value;
    _user = new(userSettings.UniqueNameSettings, uniqueName: "admin", tenantId: "16fc05de-cff6-4be8-aa3b-fb67d4e7f6a6")
    {
      Email = new("admin@test.com", isVerified: true)
    };
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct user.")]
  public async Task CreateAsync_it_should_create_the_correct_user()
  {
    await InitializeDatabaseAsync();

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
    User user = await _userService.CreateAsync(payload, _cancellationToken);
    Assert.Equal(payload.TenantId, user.TenantId);
    Assert.Equal(payload.UniqueName, user.UniqueName);
    Assert.True(user.HasPassword);
    Assert.Equal(_actor, user.PasswordChangedBy);
    Assert.NotNull(user.PasswordChangedOn);
    Assert.True(user.IsDisabled);
    Assert.Equal(_actor, user.DisabledBy);
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
    await InitializeDatabaseAsync();

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
      async () => await _userService.CreateAsync(payload, _cancellationToken));
    Assert.Equal(payload.TenantId, exception.TenantId);
    Assert.Equal(payload.Email.Address, exception.EmailAddress);
    Assert.Equal("Email", exception.PropertyName);
  }

  [Fact(DisplayName = "CreateAsync: it should throw UniqueNameAlreadyUsedException when unique name is already used.")]
  public async Task CreateAsync_it_should_throw_UniqueNameAlreadyUsedException_when_unique_name_is_already_used()
  {
    await InitializeDatabaseAsync();

    CreateUserPayload payload = new()
    {
      TenantId = _user.TenantId,
      UniqueName = _user.UniqueName
    };
    var exception = await Assert.ThrowsAsync<UniqueNameAlreadyUsedException<UserAggregate>>(
      async () => await _userService.CreateAsync(payload, _cancellationToken));
    Assert.Equal(payload.TenantId, exception.TenantId);
    Assert.Equal(payload.UniqueName, exception.UniqueName);
    Assert.Equal("UniqueName", exception.PropertyName);
  }

  private async Task InitializeDatabaseAsync(CancellationToken cancellationToken = default)
  {
    await _eventContext.Database.MigrateAsync(cancellationToken);
    await _eventContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Events];", cancellationToken);

    await _identityContext.Database.MigrateAsync(cancellationToken);
    await _identityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Users];", cancellationToken);

    await _userRepository.SaveAsync(_user, cancellationToken);
  }
}
