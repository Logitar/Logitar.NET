using FluentValidation;
using Logitar.Identity.Core;
using Logitar.Identity.Core.Tokens;
using Logitar.Identity.Core.Tokens.Models;
using Logitar.Identity.Core.Tokens.Payloads;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Logitar.Identity.IntegrationTests.Tokens;

[Trait(Traits.Category, Categories.Integration)]
public class TokenServiceTests
{
  private static readonly CancellationToken _cancellationToken = default;

  private static readonly IServiceProvider _serviceProvider;

  private readonly ITokenService _tokenService;

  static TokenServiceTests()
  {
    _serviceProvider = new ServiceCollection()
      .AddLogitarIdentityCore()
      .BuildServiceProvider();
  } // TODO(fpion): refactor

  public TokenServiceTests()
  {
    _tokenService = _serviceProvider.GetRequiredService<ITokenService>();
  }

  [Fact(DisplayName = "CreateAsync: it should create the correct token.")]
  public async Task CreateAsync_it_should_create_the_correct_token()
  {
    CreateTokenPayload payload = new()
    {
      IsConsumable = true,
      Lifetime = 24 * 60 * 60,
      Purpose = "reset_password",
      Secret = "r!s7M2n4Rq_'VdUJ<b):LCk=3KQXH]hW",
      Audience = "audience",
      Issuer = "issuer",
      Subject = Guid.NewGuid().ToString(),
      EmailAddress = "admin@test.com",
      Claims = new[]
      {
        new TokenClaim("claim_type", "claim_value", ClaimValueTypes.String)
      }
    };
    CreatedToken createdToken = await _tokenService.CreateAsync(payload, _cancellationToken);

    JwtSecurityTokenHandler tokenHandler = new();
    TokenValidationParameters parameters = new()
    {
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(payload.Secret)),
      ValidAudience = payload.Audience,
      ValidIssuer = payload.Issuer,
      ValidateAudience = true,
      ValidateIssuer = true,
      ValidateIssuerSigningKey = true
    };
    ClaimsPrincipal principal = tokenHandler.ValidateToken(createdToken.Token, parameters, out _);
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
      async () => await _tokenService.CreateAsync(payload, _cancellationToken));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "GreaterThanValidator" && e.PropertyName == "Lifetime");
    Assert.Contains(exception.Errors, e => e.ErrorCode == "MinimumLengthValidator" && e.PropertyName == "Secret");
  }
}
