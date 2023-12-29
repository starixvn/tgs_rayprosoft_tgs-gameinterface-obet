using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class FundInGetService(
    ILogger<FundInGetService> logger,
    IGameApiClient client) : IGetService<FundInQuery, FundResult>
{
    public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        try
        {
            await client.TransferFundAsync(
                apiUrl: query.ApiUrl,
                apiKey: query.ApiKey,
                agentId: query.AgentId,
                playerCode: query.PlayerCode,
                serialNo: query.SerialNo,
                amount: -query.Amount,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return new FundResult(
                SerialNo: query.SerialNo);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred with this fund.");
            throw;
        }
    }
}
