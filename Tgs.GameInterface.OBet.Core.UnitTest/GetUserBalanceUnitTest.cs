using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.OBet.Core.UnitTest;

public class GetUserBalanceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://sports-api.ob33club.com/");
		var apiKey = "G4sL7r0kKFI5Mm317w99";
		var agentId = "YYTES12";
		var playerCode = "4ucmqplxb029";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var userId = "YYTES124ucmqplxb029";
		_ = fakeGameApiUtensil
			.GetUserId(
				agentId: Arg.Is(agentId),
				playerCode: Arg.Is(playerCode))
			.Returns(userId);

		var hash = "8FC127DC25D8465E378F00E46E4429B7";
		_ = fakeCryptographyUtensil
			.Md5Encrypt(
			$"k={apiKey}&a={agentId}&u={userId}")
			.Returns(hash);

		var query = HttpUtility.ParseQueryString(string.Empty);
		query["agentId"] = agentId;
		query["userId"] = userId;
		query["hash"] = hash;

		var response = new
		{
			Status = "success",
			Data = new
			{
				Balance = 1
			},
			Message = string.Empty
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Get, "/api/sb/bal")
			.WithQueryString(query.ToString()!)
			.Respond(HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeGameApiUtensil,
			fakeCryptographyUtensil,
			fakeHttpClient);

		var actual = await client.GetUserBalanceAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentId: agentId,
			playerCode: playerCode);

		Assert.NotNull(actual);
		Assert.Equal(1m, actual.Balance);
	}

	[Fact]
	public async Task Operation_Canceled_Exception()
	{
		var apiUrl = new Uri("https://sports-api.ob33club.com/");
		var apiKey = "G4sL7r0kKFI5Mm317w99";
		var agentId = "YYTES12";
		var playerCode = "4ucmqplxb029";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var userId = "YYTES124ucmqplxb029";
		_ = fakeGameApiUtensil
			.GetUserId(
				agentId: Arg.Is(agentId),
				playerCode: Arg.Is(playerCode))
			.Returns(userId);

		var hash = "8FC127DC25D8465E378F00E46E4429B7";
		_ = fakeCryptographyUtensil
			.Md5Encrypt(
			$"k={apiKey}&a={agentId}&u={userId}")
			.Returns(hash);

		var query = HttpUtility.ParseQueryString(string.Empty);
		query["agentId"] = agentId;
		query["userId"] = userId;
		query["hash"] = hash;

		var response = new
		{
			Status = "fail",
			Data = null as object,
			Message = "NotFound"
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Get, "/api/sb/bal")
			.WithQueryString(query.ToString()!)
			.Throw(new OperationCanceledException());

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeGameApiUtensil,
			fakeCryptographyUtensil,
			fakeHttpClient);

		_ = await Assert.ThrowsAsync<OperationCanceledException>(
			() => client.GetUserBalanceAsync(
				apiUrl: apiUrl,
				apiKey: apiKey,
				agentId: agentId,
				playerCode: playerCode)
			.AsTask());
	}

	[Fact]
	public async Task Player_Does_Not_Exist()
	{
		var apiUrl = new Uri("https://sports-api.ob33club.com/");
		var apiKey = "G4sL7r0kKFI5Mm317w99";
		var agentId = "YYTES12";
		var playerCode = "4ucmqplxb029";

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeGameApiUtensil = Substitute.For<IGameApiUtensil>();
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var userId = "YYTES124ucmqplxb029";
		_ = fakeGameApiUtensil
			.GetUserId(
				agentId: Arg.Is(agentId),
				playerCode: Arg.Is(playerCode))
			.Returns(userId);

		var hash = "8FC127DC25D8465E378F00E46E4429B7";
		_ = fakeCryptographyUtensil
			.Md5Encrypt(
			$"k={apiKey}&a={agentId}&u={userId}")
			.Returns(hash);

		var query = HttpUtility.ParseQueryString(string.Empty);
		query["agentId"] = agentId;
		query["userId"] = userId;
		query["hash"] = hash;

		var response = new
		{
			Status = "fail",
			Data = null as object,
			Message = "NotFound"
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Get, "/api/sb/bal")
			.WithQueryString(query.ToString()!)
			.Respond(HttpStatusCode.NotFound, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeGameApiUtensil,
			fakeCryptographyUtensil,
			fakeHttpClient);

		var actual = await client.GetUserBalanceAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentId: agentId,
			playerCode: playerCode);

		Assert.NotNull(actual);
		Assert.False(actual.IsPlayerExist);
	}
}
