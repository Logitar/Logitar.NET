using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Payloads;
using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Sessions;
using Logitar.Security.Cryptography;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Commands;

public class RenewSessionCommandHandler : IRequestHandler<RenewSessionCommand, Session>
{
  private readonly IPasswordHelper _passwordHelper;
  private readonly ISessionQuerier _sessionQuerier;
  private readonly ISessionRepository _sessionRepository;

  public RenewSessionCommandHandler(IPasswordHelper passwordHelper, ISessionQuerier sessionQuerier,
    ISessionRepository sessionRepository)
  {
    _passwordHelper = passwordHelper;
    _sessionQuerier = sessionQuerier;
    _sessionRepository = sessionRepository;
  }

  public async Task<Session> Handle(RenewSessionCommand command, CancellationToken cancellationToken)
  {
    RenewSessionPayload payload = command.Payload;

    RefreshToken refreshToken;
    try
    {
      refreshToken = RefreshToken.Parse(payload.RefreshToken);
    }
    catch (Exception innerException)
    {
      throw new InvalidCredentialsException($"The refresh token '{payload.RefreshToken}' is not valid.", innerException);
    }

    SessionAggregate session = await _sessionRepository.LoadAsync(refreshToken.Id, cancellationToken)
      ?? throw new InvalidCredentialsException($"The session '{refreshToken.Id}' could not be found.");

    Password newSecret = _passwordHelper.Generate(SessionAggregate.SecretLength, out byte[] secretBytes);
    session.Renew(refreshToken.Secret, newSecret);

    string actorId = session.UserId.Value;
    foreach (CustomAttributeModification modification in payload.CustomAttributes)
    {
      if (modification.Value == null)
      {
        session.RemoveCustomAttribute(modification.Key, actorId);
      }
      else
      {
        session.SetCustomAttribute(modification.Key, modification.Value, actorId);
      }
    }

    await _sessionRepository.SaveAsync(session, cancellationToken);

    Session result = await _sessionQuerier.ReadAsync(session, cancellationToken);
    result.RefreshToken = new RefreshToken(session.Id, secretBytes).ToString();

    return result;
  }
}
