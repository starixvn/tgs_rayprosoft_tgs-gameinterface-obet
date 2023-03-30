using NSubstitute;
using Tgs.GameInterface.OBet.Core.ApplicationServices;

namespace Tgs.GameInterface.OBet.Core.UnitTest.ApplicationServices;

public class PlayerCreateCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new PlayerCreateCommand(
            ApiUrl: new Uri("https://sports-api.ob33club.com/"),
            ApiKey: "apiKey",
            AgentId: "agentId",
            PlayerCode: "4ucmqplxb029");

        var client = new PlayerCreateCommandService(
            fakeGameApiClient);

        await client
           .ExecuteAsync(
              command: command)
          .ConfigureAwait(false);

        await fakeGameApiClient
            .Received(1)
            .CreateAccountAsync(
                apiUrl: Arg.Is(command.ApiUrl),
                apiKey: Arg.Is(command.ApiKey),
                agentId: Arg.Is(command.AgentId),
                playerCode: Arg.Is(command.PlayerCode),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
