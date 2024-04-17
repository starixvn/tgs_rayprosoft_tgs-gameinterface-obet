using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class PlayerBalanceGetService(
	IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.GetUserBalanceAsync(
			apiUrl: query.ApiUrl,
			apiKey: query.ApiKey,
			agentId: query.AgentId,
			playerCode: query.PlayerCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceResult(
			result.IsPlayerExist,
			result.Balance);
	}
}
