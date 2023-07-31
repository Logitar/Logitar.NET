using FluentValidation;
using Logitar.Identity.Core.Tokens;
using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Tokens.Payloads;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.IntegrationTests.Tokens;

[Trait(Traits.Category, Categories.Integration)]
public class TokenFacadeTests : IntegrationTestingBase
{
  private readonly ITokenFacade _tokenFacade;

  private readonly Guid _blacklistedTokenId = Guid.NewGuid();
  private readonly string _secret;
  private readonly SecurityKey _securityKey;
  private readonly SigningCredentials _signingCredentials;
  private readonly JwtSecurityTokenHandler _tokenHandler = new();

  public TokenFacadeTests() : base()
  {
    _tokenFacade = ServiceProvider.GetRequiredService<ITokenFacade>();

    _secret = Faker.Random.String(length: 32, minChar: '!', maxChar: '~');
    _securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret));
    _signingCredentials = new(_securityKey, SecurityAlgorithms.HmacSha256);

    _tokenHandler.InboundClaimTypeMap.Clear();
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct token.")]
  public async Task CreateAsync_it_should_create_the_correct_token()
  {
    CreateTokenPayload payload = new()
    {
      IsConsumable = true,
      Lifetime = 24 * 60 * 60,
      Purpose = "reset_password",
      Secret = _secret,
      Audience = "audience",
      Issuer = "issuer",
      Subject = Guid.NewGuid().ToString(),
      EmailAddress = Faker.Person.Email,
      Claims = new[]
      {
        new TokenClaim("claim_type", "claim_value", ClaimValueTypes.String)
      }
    };
    CreatedToken createdToken = await _tokenFacade.CreateAsync(payload, CancellationToken);

    TokenValidationParameters parameters = new()
    {
      IssuerSigningKey = _securityKey,
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
    CreateTokenPayload payload = new()
    {
      Lifetime = -900,
      Secret = "n*sMkW<P{$/7hGq+"
    };
    var exception = await Assert.ThrowsAsync<ValidationException>(
      async () => await _tokenFacade.CreateAsync(payload, CancellationToken));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "GreaterThanValidator" && e.PropertyName == "Lifetime");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MinimumLengthValidator" && e.PropertyName == "Secret");
  }

  [Fact(DisplayName = "ValidateAsync: it should throw InvalidSecurityTokenPurposeException when purpose is not served.")]
  public async Task ValidateAsync_it_should_throw_InvalidSecurityTokenPurposeException_when_purpose_is_not_served()
  {
    string purpose = "reset_password";
    ClaimsIdentity subject = new();
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.Purpose, purpose));
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.Subject, Guid.NewGuid().ToString()));
    SecurityTokenDescriptor tokenDescriptor = new()
    {
      SigningCredentials = _signingCredentials,
      Subject = subject
    };
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
    string token = _tokenHandler.WriteToken(securityToken);

    ValidateTokenPayload validatePayload = new()
    {
      Token = token,
      Secret = _secret,
      Purpose = "verify_email"
    };
    var exception = await Assert.ThrowsAsync<InvalidSecurityTokenPurposeException>(
      async () => await _tokenFacade.ValidateAsync(validatePayload, consume: false, CancellationToken));
    Assert.Equal(validatePayload.Purpose, exception.RequiredPurpose);
    Assert.Equal(new[] { purpose }, exception.ActualPurposes);
  }

  [Fact(DisplayName = "ValidateAsync: it should throw SecurityTokenBlacklistedException when token is blacklisted.")]
  public async Task ValidateAsync_it_should_throw_SecurityTokenBlacklistedException_when_token_is_blacklisted()
  {
    ClaimsIdentity subject = new();
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.TokenId, _blacklistedTokenId.ToString()));
    SecurityTokenDescriptor tokenDescriptor = new()
    {
      SigningCredentials = _signingCredentials,
      Subject = subject
    };
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
    string token = _tokenHandler.WriteToken(securityToken);

    ValidateTokenPayload validatePayload = new()
    {
      Token = token,
      Secret = _secret
    };
    var exception = await Assert.ThrowsAsync<SecurityTokenBlacklistedException>(
      async () => await _tokenFacade.ValidateAsync(validatePayload, consume: false, CancellationToken));
    Assert.Equal(new[] { _blacklistedTokenId }, exception.BlacklistedIds);
  }

  [Fact(DisplayName = "ValidateAsync: it should throw ValidationException when payload is not valid.")]
  public async Task ValidateAsync_it_should_throw_ValidationException_when_payload_is_not_valid()
  {
    ValidateTokenPayload payload = new()
    {
      Token = string.Empty,
      Secret = "T!SL-W>he@GBf9Yz"
    };
    var exception = await Assert.ThrowsAsync<ValidationException>(
      async () => await _tokenFacade.ValidateAsync(payload, consume: false, CancellationToken));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "NotEmptyValidator" && e.PropertyName == "Token");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MinimumLengthValidator" && e.PropertyName == "Secret");
  }

  [Fact(DisplayName = "ValidateAsync: it should validate the correct token.")]
  public async Task ValidateAsync_it_should_validate_the_correct_token()
  {
    string userId = Guid.NewGuid().ToString();
    string emailAddress = Faker.Person.Email;
    Guid tokenId = Guid.NewGuid();
    ClaimsIdentity subject = new();
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.Audience, "audience"));
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.EmailAddress, emailAddress));
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.Issuer, "issuer"));
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.Purpose, "reset_password"));
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.Subject, userId));
    subject.AddClaim(new Claim(Rfc7519ClaimTypes.TokenId, tokenId.ToString()));
    subject.AddClaim(DateTime.UtcNow.AddDays(1).CreateClaim(Rfc7519ClaimTypes.ExpiresOn));
    SecurityTokenDescriptor tokenDescriptor = new()
    {
      SigningCredentials = _signingCredentials,
      Subject = subject
    };
    SecurityToken securityToken = _tokenHandler.CreateToken(tokenDescriptor);
    string token = _tokenHandler.WriteToken(securityToken);

    ValidateTokenPayload payload = new()
    {
      Token = token,
      Secret = _secret,
      Audience = "audience",
      Issuer = "issuer",
      Purpose = "reset_password"
    };
    ValidatedToken validatedToken = await _tokenFacade.ValidateAsync(payload, consume: true, CancellationToken);
    Assert.Equal(userId, validatedToken.Subject);
    Assert.Equal(emailAddress, validatedToken.EmailAddress);
    Assert.Contains(validatedToken.Claims, claim => claim.Type == Rfc7519ClaimTypes.TokenId && claim.Value == tokenId.ToString());

    Assert.True(await IdentityContext.TokenBlacklist.AnyAsync(x => x.Id == tokenId, CancellationToken));
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();

    BlacklistedTokenEntity blacklisted = new(_blacklistedTokenId);
    IdentityContext.TokenBlacklist.Add(blacklisted);
    await IdentityContext.SaveChangesAsync();
  }
}
