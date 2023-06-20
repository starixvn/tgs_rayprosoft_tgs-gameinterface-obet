using NSubstitute;
using Tgs.GameInterface.OBet.Core.ApplicationServices;

namespace Tgs.GameInterface.OBet.Core.UnitTest.ApplicationServices;

public class PlayerUpdateBetLimitCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new PlayerUpdateBetLimitCommand(
            ApiUrl: new Uri("https://sports-api.ob33club.com/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            PlayerCode: "4ucmqplxb029",
            MaxBetGrpHdp: 269m,
            MaxBetGrpBasketBall: 169m,
            MaxBetGrpOthersSport: 69m,
            MaxBetGrp1x2: 299m,
            MaxBetGrpMixParlay: 199m,
            MaxBetGrpOthersOdds: 109m,
            MaxBetGrpSpecial: 59m);

        var client = new PlayerUpdateBetLimitCommandService(
            fakeGameApiClient);

        await client
           .ExecuteAsync(
              command: command)
          .ConfigureAwait(false);

        await fakeGameApiClient
            .Received(1)
            .UpdateAccountAsync(
                apiUrl: Arg.Is(command.ApiUrl),
                apiKey: Arg.Is(command.ApiKey),
                agentId: Arg.Is(command.AgentId),
                playerCode: Arg.Is(command.PlayerCode),
                maxBetGrpHdp: Arg.Is(command.MaxBetGrpHdp),
                maxBetGrpBasketBall: Arg.Is(command.MaxBetGrpBasketBall),
                maxBetGrpOthersSport: Arg.Is(command.MaxBetGrpOthersSport),
                maxBetGrp1x2: Arg.Is(command.MaxBetGrp1x2),
                maxBetGrpMixParlay: Arg.Is(command.MaxBetGrpMixParlay),
                maxBetGrpOthersOdds: Arg.Is(command.MaxBetGrpOthersOdds),
                maxBetGrpSpecial: Arg.Is(command.MaxBetGrpSpecial),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
