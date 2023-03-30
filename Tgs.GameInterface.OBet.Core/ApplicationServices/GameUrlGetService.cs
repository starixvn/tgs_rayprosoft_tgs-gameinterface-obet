using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class GameUrlGetService : IGetService<GameUrlQuery, GameUrlResult>
{
    private readonly IGameApiClient _client;

    public GameUrlGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameUrlResult> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client
             .UserLoginAsync(
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
