using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class PlayerLogoutCommandService(
	IGameApiClient client) : ICommandService<PlayerLogoutCommand>
{
	public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
		=> client.UserLogoutAsync(
			apiUrl: command.ApiUrl,
			apiKey: command.ApiKey,
			agentId: command.AgentId,
			playerCode: command.PlayerCode,
			cancellationToken: cancellationToken);
}
