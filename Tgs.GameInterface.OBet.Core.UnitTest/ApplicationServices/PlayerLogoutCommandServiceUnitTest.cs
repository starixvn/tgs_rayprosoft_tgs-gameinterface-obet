using NSubstitute;
using Tgs.GameInterface.OBet.Core.ApplicationServices;

namespace Tgs.GameInterface.OBet.Core.UnitTest.ApplicationServices;

public class PlayerLogoutCommandServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var command = new PlayerLogoutCommand(
			ApiUrl: new Uri("https://sports-api.ob33club.com/"),
			ApiKey: "apiKey",
			AgentId: "agentId",
			PlayerCode: "4ucmqplxb029");

		var client = new PlayerLogoutCommandService(
			fakeGameApiClient);

		await client.ExecuteAsync(command: command);

		await fakeGameApiClient
			.Received(1)
			.UserLogoutAsync(
			apiUrl: Arg.Is(command.ApiUrl),
			apiKey: Arg.Is(command.ApiKey),
			agentId: Arg.Is(command.AgentId),
			playerCode: Arg.Is(command.PlayerCode),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
