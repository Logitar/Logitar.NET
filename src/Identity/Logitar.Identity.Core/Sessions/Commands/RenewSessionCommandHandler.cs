using Logitar.Identity.Core.Sessions.Models;
using Logitar.Identity.Core.Sessions.Payloads;
using Logitar.Identity.Domain;
using Logitar.Identity.Domain.Sessions;
using MediatR;

namespace Logitar.Identity.Core.Sessions.Commands;

public class RenewSessionCommandHandler : IRequestHandler<RenewSessionCommand, Session>
{
  private readonly ISessionQuerier _sessionQuerier;
  private readonly ISessionRepository _sessionRepository;

  public RenewSessionCommandHandler(ISessionQuerier sessionQuerier, ISessionRepository sessionRepository)
  {
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

    session.Renew(refreshToken.Secret);

    await _sessionRepository.SaveAsync(session, cancellationToken);

    Session result = await _sessionQuerier.ReadAsync(session, cancellationToken);
    if (session.Secret != null)
    {
      result.RefreshToken = new RefreshToken(session).ToString();
    }

    return result;
  }
}
