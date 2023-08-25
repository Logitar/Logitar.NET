using Bogus;
using Logitar.EventSourcing;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Sessions;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Logitar.Identity.Managers;

[Trait(Traits.Category, Categories.Integration)]
public class UserManagerTests : IntegrationTests, IAsyncLifetime
{
  private readonly Faker _faker = new();
  private readonly string _tenantId = Guid.NewGuid().ToString();

  private readonly IAggregateRepository _aggregateRepository;
  private readonly IPasswordService _passwordService;
  private readonly IUserManager _userManager;
  private readonly IUserRepository _userRepository;

  private readonly SessionAggregate _session;
  private readonly UserAggregate _user;

  public UserManagerTests()
  {
    _aggregateRepository = ServiceProvider.GetRequiredService<IAggregateRepository>();
    _passwordService = ServiceProvider.GetRequiredService<IPasswordService>();
    _userManager = ServiceProvider.GetRequiredService<IUserManager>();
    _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();

    _user = new(UserSettings.UniqueNameSettings, "admin", _tenantId)
    {
      Email = new EmailAddress(_faker.Person.Email, isVerified: true)
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

  [Fact(DisplayName = "SaveAsync: it should change the user password.")]
  public async Task SaveAsync_it_should_change_the_user_password()
  {
    string currentPassword = "Test123!";
    string newPassword = "P@s$W0rD";

    _user.SetPassword(_passwordService.Create(currentPassword));
    await _userManager.SaveAsync(_user);
    _ = _user.SignIn(UserSettings, currentPassword);

    _user.ChangePassword(currentPassword, _passwordService.Create(newPassword));
    await _userManager.SaveAsync(_user);
    _ = _user.SignIn(UserSettings, newPassword);

    var exception = Assert.Throws<IncorrectUserPasswordException>(() => _user.ChangePassword(currentPassword, _passwordService.Create(newPassword)));
    Assert.Equal(_user.ToString(), exception.User);
    Assert.Equal(currentPassword, exception.Password);

    exception = Assert.Throws<IncorrectUserPasswordException>(() => _user.SignIn(UserSettings, currentPassword));
    Assert.Equal(_user.ToString(), exception.User);
    Assert.Equal(currentPassword, exception.Password);

    UserEntity user = await IdentityContext.Users.AsNoTracking()
      .SingleAsync(x => x.AggregateId == _user.Id.Value);
    Assert.NotNull(user.Password);
    Assert.True(_passwordService.Decode(user.Password).IsMatch(newPassword));
  }

  [Fact(DisplayName = "SaveAsync: it should save the user when email unicity is not required.")]
  public async Task SaveAsync_it_should_save_the_user_when_email_unicity_is_not_required()
  {
    UserSettings? userSettings = UserSettings as UserSettings;
    Assert.NotNull(userSettings);
    userSettings.RequireUniqueEmail = false;

    Assert.NotNull(_user.Email);
    UserAggregate user = new(UserSettings.UniqueNameSettings, $"{_user.UniqueName}2", _user.TenantId)
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
    UserAggregate user = new(UserSettings.UniqueNameSettings, $"{_user.UniqueName}2", _user.TenantId)
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
    UserAggregate user = new(UserSettings.UniqueNameSettings, _user.UniqueName, _user.TenantId);
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
