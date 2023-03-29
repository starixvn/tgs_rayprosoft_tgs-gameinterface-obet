using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class FundOutGetService : IGetService<FundOutQuery, FundResult>
{
    private readonly IGameApiClient _client;
    private readonly ILogger<FundOutGetService> _logger;

    public FundOutGetService(
        ILogger<FundOutGetService> logger,
        IGameApiClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            await _client
                .TransferFundAsync(
                    apiUrl: query.ApiUrl,
                    apiKey: query.ApiKey,
                    agentId: query.AgentId,
                    playerCode: query.PlayerCode,
                    serialNo: query.SerialNo,
                    amount: query.Amount,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return new FundResult(
                SerialNo: query.SerialNo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred with this fund.");
            throw;
        }
    }
}