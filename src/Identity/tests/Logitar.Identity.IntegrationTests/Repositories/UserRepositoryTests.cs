using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Roles;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Logitar.Identity.Repositories;

[Trait(Traits.Category, Categories.Integration)]
public class UserRepositoryTests : IntegrationTestBase, IAsyncLifetime
{
  private readonly string _password = "P@s$W0rD";
  private readonly string _tenantId = Guid.NewGuid().ToString();

  private readonly IAggregateRepository _aggregateRepository;
  private readonly IPasswordService _passwordService;
  private readonly IOptions<RoleSettings> _roleSettings;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  private readonly UserAggregate _admin;
  private readonly UserAggregate _other;
  private readonly UserAggregate _disabled;
  private readonly UserAggregate _deleted;
  private readonly UserAggregate _noTenant;
  private readonly RoleAggregate _role;

  public UserRepositoryTests() : base()
  {
    _aggregateRepository = ServiceProvider.GetRequiredService<IAggregateRepository>();
    _passwordService = ServiceProvider.GetRequiredService<IPasswordService>();
    _roleSettings = ServiceProvider.GetRequiredService<IOptions<RoleSettings>>();
    _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
    _userSettings = ServiceProvider.GetRequiredService<IOptions<UserSettings>>();

    _role = new(_roleSettings.Value.UniqueNameSettings, "admin", _tenantId);

    UserSettings userSettings = _userSettings.Value;

    _admin = new(userSettings.UniqueNameSettings, "admin", _tenantId)
    {
      Address = new PostalAddress("Fondation René-Lévesque\r\nCP 47524, succ. Plateau Mont-Royal", "Montréal", "CA", "QC", "H2H 2S8", isVerified: true),
      Email = new EmailAddress("info@fondationrene-levesque.org", isVerified: true),
      Phone = new PhoneNumber("4186434408", "CA", "47524"),
      FirstName = "René",
      MiddleName = "Charles",
      LastName = "Lévesque",
      Nickname = "Ti-poil",
      Birthdate = new DateTime(1922, 8, 24),
      Gender = new Gender("Male"),
      Locale = CultureInfo.GetCultureInfo("fr-CA"),
      TimeZone = new TimeZoneEntry("America/Toronto"),
      Picture = new Uri("https://upload.wikimedia.org/wikipedia/commons/a/a9/Ren%C3%A9_L%C3%A9vesque%2C_18_octobre_1960.jpg"),
      Profile = new Uri("https://fr.wikipedia.org/wiki/Ren%C3%A9_L%C3%A9vesque"),
      Website = new Uri("https://fondationrene-levesque.org/")
    };
    _admin.AddRole(_role);
    _admin.Disable();
    _admin.Enable();
    _admin.SetCustomAttribute("Initials", "CRL");
    _admin.SetPassword(_passwordService.Create(_password));
    _admin.SignIn(userSettings, password: null);

    _other = new(userSettings.UniqueNameSettings, "other", _tenantId);

    _disabled = new(userSettings.UniqueNameSettings, "disabled", _tenantId);
    _disabled.Disable();

    _deleted = new(userSettings.UniqueNameSettings, "deleted", _tenantId);
    _deleted.Delete();

    _noTenant = new(userSettings.UniqueNameSettings, _admin.UniqueName, tenantId: null);
  }

  [Fact(DisplayName = "LoadAsync: it loads the correct user by unique name.")]
  public async Task LoadAsync_it_loads_the_correct_user_by_unique_name()
  {
    UserAggregate? user = await _userRepository.LoadAsync(tenantId: null, _noTenant.UniqueName);
    Assert.NotNull(user);
    Assert.Equal(_noTenant, user);
  }

  [Fact(DisplayName = "LoadAsync: it loads the correct users by email.")]
  public async Task LoadAsync_it_loads_the_correct_users_by_email()
  {
    Assert.NotNull(_admin.Email);
    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(_tenantId, _admin.Email);
    UserAggregate user = Assert.Single(users);
    Assert.Equal(_admin, user);
  }

  [Fact(DisplayName = "LoadAsync: it loads the correct users by tenant.")]
  public async Task LoadAsync_it_loads_the_correct_users_by_tenant()
  {
    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(_tenantId);
    Assert.Equal(3, users.Count());
    Assert.Contains(users, user => user.Equals(_admin));
    Assert.Contains(users, user => user.Equals(_other));
    Assert.Contains(users, user => user.Equals(_disabled));
  }

  [Fact(DisplayName = "LoadAsync: it loads the correct users by role.")]
  public async Task LoadAsync_it_loads_the_correct_users_by_role()
  {
    IEnumerable<UserAggregate> users = await _userRepository.LoadAsync(_role);
    UserAggregate user = Assert.Single(users);
    Assert.Equal(_admin, user);
  }

  [Fact(DisplayName = "SaveAsync: it saves the correct user.")]
  public async Task SaveAsync_it_saves_the_correct_user()
  {
    Dictionary<string, UserEntity> users = await IdentityContext.Users.AsNoTracking()
      .Include(x => x.Roles)
      .ToDictionaryAsync(x => x.AggregateId, x => x);

    IEnumerable<UserAggregate> aggregates = new[] { _admin, _other, _disabled, _deleted, _noTenant };
    foreach (UserAggregate aggregate in aggregates)
    {
      if (aggregate.IsDeleted)
      {
        Assert.False(users.ContainsKey(aggregate.Id.Value));
      }
      else
      {
        UserEntity user = users[aggregate.Id.Value];

        Assert.Equal(aggregate.CreatedBy.Value, user.CreatedBy);
        AssertEqual(aggregate.CreatedOn, user.CreatedOn);
        Assert.Equal(aggregate.UpdatedBy.Value, user.UpdatedBy);
        AssertEqual(aggregate.UpdatedOn, user.UpdatedOn);
        Assert.Equal(aggregate.Version, user.Version);

        Assert.Equal(aggregate.TenantId, user.TenantId);
        Assert.Equal(aggregate.UniqueName, user.UniqueName);
        Assert.Equal(aggregate.HasPassword, user.HasPassword);
        Assert.Equal(aggregate.IsDisabled, user.IsDisabled);
        Assert.Equal(aggregate.Address?.Street, user.AddressStreet);
        Assert.Equal(aggregate.Address?.Locality, user.AddressLocality);
        Assert.Equal(aggregate.Address?.Region, user.AddressRegion);
        Assert.Equal(aggregate.Address?.PostalCode, user.AddressPostalCode);
        Assert.Equal(aggregate.Address?.Country, user.AddressCountry);
        Assert.Equal(aggregate.Address?.IsVerified ?? false, user.IsAddressVerified);
        Assert.Equal(aggregate.Email?.Address, user.EmailAddress);
        Assert.Equal(aggregate.Email?.IsVerified ?? false, user.IsEmailVerified);
        Assert.Equal(aggregate.Phone?.CountryCode, user.PhoneCountryCode);
        Assert.Equal(aggregate.Phone?.Number, user.PhoneNumber);
        Assert.Equal(aggregate.Phone?.Extension, user.PhoneExtension);
        Assert.Equal(aggregate.Phone?.IsVerified ?? false, user.IsPhoneVerified);
        Assert.Equal(aggregate.IsConfirmed, user.IsConfirmed);
        Assert.Equal(aggregate.FirstName, user.FirstName);
        Assert.Equal(aggregate.MiddleName, user.MiddleName);
        Assert.Equal(aggregate.LastName, user.LastName);
        Assert.Equal(aggregate.FullName, user.FullName);
        Assert.Equal(aggregate.Nickname, user.Nickname);
        AssertEqual(aggregate.Birthdate, user.Birthdate);
        Assert.Equal(aggregate.Gender?.Value, user.Gender);
        Assert.Equal(aggregate.Locale?.Name, user.Locale);
        Assert.Equal(aggregate.TimeZone?.Id, user.TimeZone);
        Assert.Equal(aggregate.Picture?.ToString(), user.Picture);
        Assert.Equal(aggregate.Profile?.ToString(), user.Profile);
        Assert.Equal(aggregate.Website?.ToString(), user.Website);
        Assert.Equal(aggregate.CustomAttributes, user.CustomAttributes);

        Assert.Equal(aggregate.Roles.Count, user.Roles.Count);
        foreach (AggregateId roleId in aggregate.Roles)
        {
          Assert.Contains(user.Roles, role => role.AggregateId == roleId.Value);
        }

        if (aggregate.HasPassword)
        {
          Assert.NotNull(user.Password);
          Password password = _passwordService.Decode(user.Password);
          Assert.True(password.IsMatch(_password));
        }
      }
    }
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    await _aggregateRepository.SaveAsync(new AggregateRoot[] { _role, _admin, _other, _disabled, _deleted, _noTenant });
  }
}
