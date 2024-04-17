using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class PlayerUpdateBetLimitCommandService(
	IGameApiClient client) : ICommandService<PlayerUpdateBetLimitCommand>
{
	public ValueTask ExecuteAsync(PlayerUpdateBetLimitCommand command, CancellationToken cancellationToken = default)
		=> client.UpdateAccountAsync(
			apiUrl: command.ApiUrl,
			apiKey: command.ApiKey,
			agentId: command.AgentId,
			playerCode: command.PlayerCode,
			maxBetGrpHdp: command.MaxBetGrpHdp,
			maxBetGrpBasketBall: command.MaxBetGrpBasketBall,
			maxBetGrpOthersSport: command.MaxBetGrpOthersSport,
			maxBetGrp1x2: command.MaxBetGrp1x2,
			maxBetGrpMixParlay: command.MaxBetGrpMixParlay,
			maxBetGrpOthersOdds: command.MaxBetGrpOthersOdds,
			maxBetGrpSpecial: command.MaxBetGrpSpecial,
			cancellationToken: cancellationToken);
}
