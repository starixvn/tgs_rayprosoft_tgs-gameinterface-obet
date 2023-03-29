using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class PlayerLogoutCommandService : ICommandService<PlayerLogoutCommand>
{
    private readonly IGameApiClient _client;

    public PlayerLogoutCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
        => _client
        .UserLogoutAsync(
            apiUrl: command.ApiUrl,
            apiKey: command.ApiKey,
            agentId: command.AgentId,
            playerCode: command.PlayerCode,
            cancellationToken: cancellationToken);
}
