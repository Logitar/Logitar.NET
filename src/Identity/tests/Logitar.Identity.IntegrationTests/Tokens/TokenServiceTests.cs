using FluentValidation;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Identity.Core.Models;
using Logitar.Identity.Core.Tokens;
using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Tokens.Payloads;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Domain.Users;
using Logitar.Identity.EntityFrameworkCore.SqlServer;
using Logitar.Identity.EntityFrameworkCore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.IntegrationTests.Tokens;

[Trait(Traits.Category, Categories.Integration)]
public class TokenServiceTests
{
  private const string Secret = "r!s7M2n4Rq_'VdUJ<b):LCk=3KQXH]hW";

  private static readonly Actor _actor = new();
  private static readonly Guid _blacklistedTokenId = Guid.NewGuid();
  private static readonly CancellationToken _cancellationToken = default;
  private static readonly SymmetricSecurityKey _signingKey = new(Encoding.ASCII.GetBytes(Secret));

  private static readonly IServiceProvider _serviceProvider;

  private readonly JwtSecurityTokenHandler _tokenHandler = new();

  private readonly EventContext _eventContext;
  private readonly IdentityContext _identityContext;

  private readonly ITokenService _tokenService;
  private readonly IUserRepository _userRepository;
  private readonly IOptions<UserSettings> _userSettings;

  private readonly UserAggregate _user;

  static TokenServiceTests()
  {
    IConfiguration configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json")
      .Build();

    string connectionString = configuration.GetValue<string>("SQLCONNSTR_IntegrationTests") ?? string.Empty;

    _serviceProvider = new ServiceCollection()
      .AddLogitarIdentityWithEntityFrameworkCoreSqlServer(connectionString)
      .AddSingleton<ICurrentActor>(new CurrentActorMock(_actor))
      .BuildServiceProvider();
  } // TODO(fpion): refactor

  public TokenServiceTests()
  {
    _eventContext = _serviceProvider.GetRequiredService<EventContext>();
    _identityContext = _serviceProvider.GetRequiredService<IdentityContext>();

    _tokenService = _serviceProvider.GetRequiredService<ITokenService>();
    _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
    _userSettings = _serviceProvider.GetRequiredService<IOptions<UserSettings>>();

    UserSettings userSettings = _userSettings.Value;
    _user = new(userSettings.UniqueNameSettings, uniqueName: "admin", tenantId: "16fc05de-cff6-4be8-aa3b-fb67d4e7f6a6")
    {
      Email = new("admin@test.com", isVerified: true)
    };
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct token.")]
  public async Task CreateAsync_it_should_create_the_correct_token()
  {
    await InitializeDatabaseAsync();

    Assert.NotNull(_user.Email);
    CreateTokenPayload payload = new()
    {
      IsConsumable = true,
      Lifetime = 24 * 60 * 60,
      Purpose = "reset_password",
      Secret = Secret,
      Audience = "audience",
      Issuer = "issuer",
      Subject = _user.Id.Value,
      EmailAddress = _user.Email.Address,
      Claims = new[]
      {
        new TokenClaim("claim_type", "claim_value", ClaimValueTypes.String)
      }
    };
    CreatedToken createdToken = await _tokenService.CreateAsync(payload, _cancellationToken);

    TokenValidationParameters parameters = new()
    {
      IssuerSigningKey = _signingKey,
      ValidAudience = payload.Audience,
      ValidIssuer = payload.Issuer,
      ValidateAudience = true,
      ValidateIssuer = true,
      ValidateIssuerSigningKey = true
    };
    ClaimsPrincipal principal = _tokenHandler.ValidateToken(createdToken.Token, parameters, out _);
    Dictionary<string, Claim> claims = principal.Claims.ToDictionary(claim => claim.Type, claim => claim);
    Assert.True(claims.ContainsKey(Rfc7519ClaimTypes.TokenId));
    Assert.Equal(payload.Purpose, claims[Rfc7519ClaimTypes.Purpose].Value);
    Assert.Equal(payload.Subject, claims[Rfc7519ClaimTypes.Subject].Value);
    Assert.Equal(payload.EmailAddress, claims[Rfc7519ClaimTypes.EmailAddress].Value);

    DateTime expiresOn = claims[Rfc7519ClaimTypes.ExpiresOn].ParseDateTime();
    Assert.True((DateTime.UtcNow - expiresOn) < TimeSpan.FromMinutes(1));

    foreach (TokenClaim claim in payload.Claims)
    {
      Assert.Equal(claim.Value, claims[claim.Type].Value);
    }
  }

  [Fact(DisplayName = "CreateAsync: it should throw ValidationException when payload is not valid.")]
  public async Task CreateAsync_it_should_throw_ValidationException_when_payload_is_not_valid()
  {
    await InitializeDatabaseAsync();

    CreateTokenPayload payload = new()
    {
      Lifetime = -900,
      Secret = "n*sMkW<P{$/7hGq+"
    };
    var exception = await Assert.ThrowsAsync<ValidationException>(
      async () => await _tokenService.CreateAsync(payload, _cancellationToken));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "GreaterThanValidator" && e.PropertyName == "Lifetime");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MinimumLengthValidator" && e.PropertyName == "Secret");
  }

  [Fact(DisplayName = "ValidateAsync: it should throw InvalidSecurityTokenPurposeException when purpose is not served.")]
  public async Task ValidateAsync_it_should_throw_InvalidSecurityTokenPurposeException_when_purpose_is_not_served()
  {
    await InitializeDatabaseAsync();

    string purpose = "reset_password";
    ClaimsIdentity subject = new();
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.Purpose, purpose));
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.Subject, _user.Id.Value));
    SecurityTokenDescriptor tokenDescriptor = new()
    {
      SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256),
      Subject = subject
    };
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
    string token = _tokenHandler.WriteToken(securityToken);

    ValidateTokenPayload validatePayload = new()
    {
      Token = token,
      Secret = Secret,
      Purpose = "verify_email"
    };
    var exception = await Assert.ThrowsAsync<InvalidSecurityTokenPurposeException>(
      async () => await _tokenService.ValidateAsync(validatePayload, consume: false, _cancellationToken));
    Assert.Equal(validatePayload.Purpose, exception.RequiredPurpose);
    Assert.Equal(new[] { purpose }, exception.ActualPurposes);
  }

  [Fact(DisplayName = "ValidateAsync: it should throw SecurityTokenBlacklistedException when token is blacklisted.")]
  public async Task ValidateAsync_it_should_throw_SecurityTokenBlacklistedException_when_token_is_blacklisted()
  {
    await InitializeDatabaseAsync();

    ClaimsIdentity subject = new();
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.TokenId, _blacklistedTokenId.ToString()));
    SecurityTokenDescriptor tokenDescriptor = new()
    {
      SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256),
      Subject = subject
    };
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
    string token = _tokenHandler.WriteToken(securityToken);

    ValidateTokenPayload validatePayload = new()
    {
      Token = token,
      Secret = Secret
    };
    var exception = await Assert.ThrowsAsync<SecurityTokenBlacklistedException>(
      async () => await _tokenService.ValidateAsync(validatePayload, consume: false, _cancellationToken));
    Assert.Equal(new[] { _blacklistedTokenId }, exception.BlacklistedIds);
  }

  [Fact(DisplayName = "ValidateAsync: it should throw ValidationException when payload is not valid.")]
  public async Task ValidateAsync_it_should_throw_ValidationException_when_payload_is_not_valid()
  {
    await InitializeDatabaseAsync();

    ValidateTokenPayload payload = new()
    {
      Token = string.Empty,
      Secret = "T!SL-W>he@GBf9Yz"
    };
    var exception = await Assert.ThrowsAsync<ValidationException>(
      async () => await _tokenService.ValidateAsync(payload, consume: false, _cancellationToken));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "Token");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MinimumLengthValidator" && e.PropertyName == "Secret");
  }

  [Fact(DisplayName = "ValidateAsync: it should validate the correct token.")]
  public async Task ValidateAsync_it_should_validate_the_correct_token()
  {
    await InitializeDatabaseAsync();

    Assert.NotNull(_user.Email);
    Guid tokenId = Guid.NewGuid();
    ClaimsIdentity subject = new();
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.Audience, "audience"));
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.EmailAddress, _user.Email.Address));
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.Issuer, "issuer"));
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.Purpose, "reset_password"));
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.Subject, _user.Id.Value));
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.TokenId, tokenId.ToString()));
    subject.AddClaim(DateTime.UtcNow.AddDays(1).CreateClaim(Rfc7519ClaimTypes.ExpiresOn));
    SecurityTokenDescriptor tokenDescriptor = new()
    {
      SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256),
      Subject = subject
    };
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
    string token = _tokenHandler.WriteToken(securityToken);

    ValidateTokenPayload payload = new()
    {
      Token = token,
      Secret = Secret,
      Audience = "audience",
      Issuer = "issuer",
      Purpose = "reset_password"
    };
    ValidatedToken validatedToken = await _tokenService.ValidateAsync(payload, consume: true, _cancellationToken);
    Assert.Equal(_user.Id.Value, validatedToken.Subject);
    Assert.Equal(_user.Email.Address, validatedToken.EmailAddress);
    Assert.Contains(validatedToken.Claims, claim => claim.Type == Rfc7519ClaimTypes.TokenId && claim.Value == tokenId.ToString());

    Assert.True(await _identityContext.TokenBlacklist.AnyAsync(x => x.Id == tokenId, _cancellationToken));
  }

  private async Task InitializeDatabaseAsync(CancellationToken cancellationToken = default)
  {
    await _eventContext.Database.MigrateAsync(cancellationToken);
    await _eventContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Events];", cancellationToken);

    await _identityContext.Database.MigrateAsync(cancellationToken);
    await _identityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Users];", cancellationToken);
    await _identityContext.Database.ExecuteSqlRawAsync("DELETE FROM [dbo].[Tokenblacklist];", cancellationToken);

    await _userRepository.SaveAsync(_user, cancellationToken);

    BlacklistedTokenEntity blacklisted = new(_blacklistedTokenId);
    _identityContext.TokenBlacklist.Add(blacklisted);
    await _identityContext.SaveChangesAsync(cancellationToken);
  }
}
