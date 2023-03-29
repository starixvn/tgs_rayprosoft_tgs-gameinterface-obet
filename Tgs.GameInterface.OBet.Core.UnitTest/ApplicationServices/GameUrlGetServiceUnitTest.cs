using NSubstitute;
using Tgs.GameInterface.OBet.Core.ApplicationServices;
using Tgs.GameInterface.OBet.Core.Models;

namespace Tgs.GameInterface.OBet.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameUrlQuery(
            ApiUrl: new Uri("https://sports-api.ob33club.com/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            PlayerCode: "4ucmqplxb029",
            Lang: "zh-cn",
            IsMobile: false);

        var result = new UserLoginResult(
            IsPlayerExist: true,
            GameUrl: "http://localhost:8088/");
        _ = fakeGameApiClient
            .UserLoginAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 apiKey: Arg.Is(query.ApiKey),
                 agentId: Arg.Is(query.AgentId),
                 playerCode: Arg.Is(query.PlayerCode),
                 lang: Arg.Is(query.Lang),
                 isMobile: Arg.Is(query.IsMobile),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(result);

        var client = new GameUrlGetService(
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.IsPlayerExist);
    }
}
