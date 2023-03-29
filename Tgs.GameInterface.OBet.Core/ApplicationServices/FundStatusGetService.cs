using Tgs.ServiceUtility;

namespace Tgs.GameInterface.OBet.Core.ApplicationServices;

internal class FundStatusGetService : IGetService<FundStatusQuery, bool>
{
    private readonly IGameApiClient _client;

    public FundStatusGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
        => _client
        .TransferFundRecordEnquiryAsync(
            apiUrl: query.ApiUrl,
            apiKey: query.ApiKey,
            agentId: query.AgentId,
            playerCode: query.PlayerCode,
            serialNo: query.SerialNo,
            cancellationToken: cancellationToken);
}