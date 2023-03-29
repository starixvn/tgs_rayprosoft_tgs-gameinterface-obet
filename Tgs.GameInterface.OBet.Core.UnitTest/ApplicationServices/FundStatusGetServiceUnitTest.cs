using NSubstitute;
using Tgs.GameInterface.OBet.Core.ApplicationServices;

namespace Tgs.GameInterface.OBet.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundStatusQuery(
            ApiUrl: new Uri("https://sports-api.ob33club.com/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            PlayerCode: "4ucmqplxb029",
            SerialNo: Guid.NewGuid().ToString("N"));

        _ = fakeGameApiClient
            .TransferFundRecordEnquiryAsync(
                  apiUrl: Arg.Is(query.ApiUrl),
                  apiKey: Arg.Is(query.ApiKey),
                  agentId: Arg.Is(query.AgentId),
                  playerCode: Arg.Is(query.PlayerCode),
                  serialNo: Arg.Is(query.SerialNo),
                  cancellationToken: Arg.Any<CancellationToken>())
            .Returns(true);

        var client = new FundStatusGetService(
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.True(actual);
    }
}
