using Logitar.EventSourcing;
using Logitar.Identity.Core.ApiKeys.Models;
using MediatR;

namespace Logitar.Identity.Core.ApiKeys.Commands;

/// <summary>
/// Represents the handler for instances of <see cref="DeleteApiKeyCommand"/>.
/// </summary>
internal class DeleteApiKeyCommandHandler : IRequestHandler<DeleteApiKeyCommand, ApiKey?>
{
  /// <summary>
  /// The API key repository.
  /// </summary>
  private readonly IApiKeyRepository _apiKeyRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="DeleteApiKeyCommandHandler"/> class.
  /// </summary>
  /// <param name="apiKeyRepository">The API key repository.</param>
  public DeleteApiKeyCommandHandler(IApiKeyRepository apiKeyRepository)
  {
    _apiKeyRepository = apiKeyRepository;
  }

  /// <summary>
  /// Handles the specified request.
  /// </summary>
  /// <param name="request">The request to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The resulting read model.</returns>
  public async Task<ApiKey?> Handle(DeleteApiKeyCommand request, CancellationToken cancellationToken)
  {
    AggregateId id = request.Id.RequireAggregateId(nameof(request.Id));
    ApiKeyAggregate? apiKey = await _apiKeyRepository.LoadAsync(id, cancellationToken);
    if (apiKey == null)
    {
      return null;
    }
    ApiKey result = await _apiKeyRepository.ReadAsync(apiKey, cancellationToken);

    apiKey.Delete();

    await _apiKeyRepository.SaveAsync(apiKey, cancellationToken);

    return result;
  }
}
