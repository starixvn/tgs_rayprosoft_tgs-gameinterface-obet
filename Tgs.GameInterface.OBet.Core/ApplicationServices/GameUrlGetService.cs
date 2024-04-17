using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class GameUrlGetService(
	IGameApiClient client) : IGetService<GameUrlQuery, GameUrlResult>
{
	public async ValueTask<GameUrlResult> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client.UserLoginAsync(
			apiUrl: query.ApiUrl,
			apiKey: query.ApiKey,
			agentId: query.AgentId,
			playerCode: query.PlayerCode,
			lang: query.Lang,
			isMobile: query.IsMobile,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new GameUrlResult(
			IsPlayerExist: result.IsPlayerExist,
			GameUrl: result.GameUrl);
	}
}
