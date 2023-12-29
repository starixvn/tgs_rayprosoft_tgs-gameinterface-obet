using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class PlayerCreateCommandService(
    IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
    public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => client.CreateAccountAsync(
            apiUrl: command.ApiUrl,
            apiKey: command.ApiKey,
            agentId: command.AgentId,
            playerCode: command.PlayerCode,
            cancellationToken: cancellationToken);
}
