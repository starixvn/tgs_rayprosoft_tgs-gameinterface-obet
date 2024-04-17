using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class FundOutGetService(
	ILogger<FundOutGetService> logger,
	IGameApiClient client) : IGetService<FundOutQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
	{
		try
		{
			await client.TransferFundAsync(
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
			logger.LogError(ex, "An exception occurred with this fund.");
			throw;
		}
	}
}
