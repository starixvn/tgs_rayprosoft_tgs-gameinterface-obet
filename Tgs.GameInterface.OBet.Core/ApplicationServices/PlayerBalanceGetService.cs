using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class PlayerBalanceGetService : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    private readonly IGameApiClient _client;

    public PlayerBalanceGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client
            .GetUserBalanceAsync(
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
