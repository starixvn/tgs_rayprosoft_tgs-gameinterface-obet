using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class PlayerCreateCommandService : ICommandService<PlayerCreateCommand>
{
    private readonly IGameApiClient _client;

    public PlayerCreateCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => _client
        .CreateAccountAsync(
            apiUrl: command.ApiUrl,
            apiKey: command.ApiKey,
            agentId: command.AgentId,
            playerCode: command.PlayerCode,
            cancellationToken: cancellationToken);
}