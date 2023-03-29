using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.GameInterface.OBet.Core.ApplicationServices;

namespace Tgs.GameInterface.OBet.Core.UnitTest.ApplicationServices;

public class FundInGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeLog = NullLogger<FundInGetService>.Instance;
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundInQuery(
            ApiUrl: new Uri("http://api.fcg666.net/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            PlayerCode: "4ucmqplxb029",
            SerialNo: Guid.NewGuid().ToString("N"),
            Amount: 1);

        var client = new FundInGetService(
            fakeLog,
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(query.SerialNo, actual.SerialNo);

        await fakeGameApiClient
             .Received(1)
             .TransferFundAsync(
                  apiUrl: Arg.Is(query.ApiUrl),
                  apiKey: Arg.Is(query.ApiKey),
                  agentId: Arg.Is(query.AgentId),
                  playerCode: Arg.Is(query.PlayerCode),
                  serialNo: Arg.Is(query.SerialNo),
                  amount: Arg.Is(-query.Amount),
                  cancellationToken: Arg.Any<CancellationToken>())
             .ConfigureAwait(false);
    }
}