using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class FundStatusGetService(
    IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
    public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
        => client
        .TransferFundRecordEnquiryAsync(
            apiUrl: query.ApiUrl,
            apiKey: query.ApiKey,
            agentId: query.AgentId,
            playerCode: query.PlayerCode,
            serialNo: query.SerialNo,
            cancellationToken: cancellationToken);
}
