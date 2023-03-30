using NSubstitute;
using Tgs.GameInterface.OBet.Core.ApplicationServices;
using Tgs.GameInterface.OBet.Core.Models;

namespace Tgs.GameInterface.OBet.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new PlayerBalanceQuery(
            ApiUrl: new Uri("https://sports-api.ob33club.com/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            PlayerCode: "4ucmqplxb029");

        var result = new BalanceResult(
            IsPlayerExist: true,
            Balance: 1m);
        _ = fakeGameApiClient
            .GetUserBalanceAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 apiKey: Arg.Is(query.ApiKey),
                 agentId: Arg.Is(query.AgentId),
                 playerCode: Arg.Is(query.PlayerCode),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(result);

        var client = new PlayerBalanceGetService(
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.IsPlayerExist);
    }
}
