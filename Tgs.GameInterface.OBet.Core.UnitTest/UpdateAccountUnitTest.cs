using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.OBet.Core.UnitTest;

public class UpdateAccountUnitTest
{
	[Fact]
	public async Task Exception()
	{
		var apiUrl = new Uri("https://sports-api.ob33club.com/");
		var apiKey = "G4sL7r0kKFI5Mm317w99";
		var agentId = "YYTES12";
		var playerCode = "4ucmqplxb029";
		var maxBetGrpHdp = 269m;
		var maxBetGrpBasketBall = 169m;
		var maxBetGrpOthersSport = 69m;
		var maxBetGrp1x2 = 299m;
		var maxBetGrpMixParlay = 199m;
		var maxBetGrpOthersOdds = 109m;
		var maxBetGrpSpecial = 59m;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
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

		var parameters = new JsonObject
		{
			{ "hash", hash },
			{ "agentId", agentId },
			{ "userId", userId },
			{ "maxbetgrphdp", maxBetGrpHdp },
			{ "maxbetgrpbasketball", maxBetGrpBasketBall },
			{ "maxbetgrpotherssport", maxBetGrpOthersSport },
			{ "maxbetgrp1x2", maxBetGrp1x2 },
			{ "maxbetgrpmixparlay", maxBetGrpMixParlay },
			{ "maxbetgrpothersodds", maxBetGrpOthersOdds },
			{ "maxbetgrpspecial", maxBetGrpSpecial }
		};
		var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

		var response = new
		{
			Status = "NotFound",
			data = null as object
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/api/sb/register")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(statusCode: HttpStatusCode.Unauthorized, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeGameApiUtensil,
			fakeCryptographyUtensil,
			fakeHttpClient);

		_ = await Assert.ThrowsAsync<Exception>(
			() => client.UpdateAccountAsync(
				apiUrl: apiUrl,
				apiKey: apiKey,
				agentId: agentId,
				playerCode: playerCode,
				maxBetGrpHdp: maxBetGrpHdp,
				maxBetGrpBasketBall: maxBetGrpBasketBall,
				maxBetGrpOthersSport: maxBetGrpOthersSport,
				maxBetGrp1x2: maxBetGrp1x2,
				maxBetGrpMixParlay: maxBetGrpMixParlay,
				maxBetGrpOthersOdds: maxBetGrpOthersOdds,
				maxBetGrpSpecial: maxBetGrpSpecial)
			.AsTask());
	}

	[Fact]
	public async Task MaxBetGrp1x2_Is_0()
	{
		var apiUrl = new Uri("https://sports-api.ob33club.com/");
		var apiKey = "G4sL7r0kKFI5Mm317w99";
		var agentId = "YYTES12";
		var playerCode = "4ucmqplxb029";
		var maxBetGrpHdp = 269m;
		var maxBetGrpBasketBall = 169m;
		var maxBetGrpOthersSport = 69m;
		var maxBetGrp1x2 = 0m;
		var maxBetGrpMixParlay = 199m;
		var maxBetGrpOthersOdds = 109m;
		var maxBetGrpSpecial = 59m;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
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

		var parameters = new JsonObject
		{
			{ "hash", hash },
			{ "agentId", agentId },
			{ "userId", userId },
			{ "maxbetgrphdp", maxBetGrpHdp },
			{ "maxbetgrpbasketball", maxBetGrpBasketBall },
			{ "maxbetgrpotherssport", maxBetGrpOthersSport },
			{ "maxbetgrpmixparlay", maxBetGrpMixParlay },
			{ "maxbetgrpothersodds", maxBetGrpOthersOdds },
			{ "maxbetgrpspecial", maxBetGrpSpecial }
		};

		var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

		var response = new
		{
			Status = "success",
			Data = null as object,
			Message = string.Empty
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/api/sb/register")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(statusCode: HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeGameApiUtensil,
			fakeCryptographyUtensil,
			fakeHttpClient);

		await client.UpdateAccountAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentId: agentId,
			playerCode: playerCode,
			maxBetGrpHdp: maxBetGrpHdp,
			maxBetGrpBasketBall: maxBetGrpBasketBall,
			maxBetGrpOthersSport: maxBetGrpOthersSport,
			maxBetGrp1x2: maxBetGrp1x2,
			maxBetGrpMixParlay: maxBetGrpMixParlay,
			maxBetGrpOthersOdds: maxBetGrpOthersOdds,
			maxBetGrpSpecial: maxBetGrpSpecial);
	}

	[Fact]
	public async Task MaxBetGrpBasketBall_Is_0()
	{
		var apiUrl = new Uri("https://sports-api.ob33club.com/");
		var apiKey = "G4sL7r0kKFI5Mm317w99";
		var agentId = "YYTES12";
		var playerCode = "4ucmqplxb029";
		var maxBetGrpHdp = 269m;
		var maxBetGrpBasketBall = 0m;
		var maxBetGrpOthersSport = 69m;
		var maxBetGrp1x2 = 299m;
		var maxBetGrpMixParlay = 199m;
		var maxBetGrpOthersOdds = 109m;
		var maxBetGrpSpecial = 59m;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
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

		var parameters = new JsonObject
		{
			{ "hash", hash },
			{ "agentId", agentId },
			{ "userId", userId },
			{ "maxbetgrphdp", maxBetGrpHdp },
			{ "maxbetgrpotherssport", maxBetGrpOthersSport },
			{ "maxbetgrp1x2", maxBetGrp1x2 },
			{ "maxbetgrpmixparlay", maxBetGrpMixParlay },
			{ "maxbetgrpothersodds", maxBetGrpOthersOdds },
			{ "maxbetgrpspecial", maxBetGrpSpecial }
		};

		var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

		var response = new
		{
			Status = "success",
			Data = null as object,
			Message = string.Empty
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/api/sb/register")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(statusCode: HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeGameApiUtensil,
			fakeCryptographyUtensil,
			fakeHttpClient);

		await client.UpdateAccountAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentId: agentId,
			playerCode: playerCode,
			maxBetGrpHdp: maxBetGrpHdp,
			maxBetGrpBasketBall: maxBetGrpBasketBall,
			maxBetGrpOthersSport: maxBetGrpOthersSport,
			maxBetGrp1x2: maxBetGrp1x2,
			maxBetGrpMixParlay: maxBetGrpMixParlay,
			maxBetGrpOthersOdds: maxBetGrpOthersOdds,
			maxBetGrpSpecial: maxBetGrpSpecial);
	}

	[Fact]
	public async Task MaxBetGrpHdp_Is_0()
	{
		var apiUrl = new Uri("https://sports-api.ob33club.com/");
		var apiKey = "G4sL7r0kKFI5Mm317w99";
		var agentId = "YYTES12";
		var playerCode = "4ucmqplxb029";
		var maxBetGrpHdp = 0m;
		var maxBetGrpBasketBall = 169m;
		var maxBetGrpOthersSport = 69m;
		var maxBetGrp1x2 = 299m;
		var maxBetGrpMixParlay = 199m;
		var maxBetGrpOthersOdds = 109m;
		var maxBetGrpSpecial = 59m;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
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

		var parameters = new JsonObject
		{
			{ "hash", hash },
			{ "agentId", agentId },
			{ "userId", userId },
			{ "maxbetgrpbasketball", maxBetGrpBasketBall },
			{ "maxbetgrpotherssport", maxBetGrpOthersSport },
			{ "maxbetgrp1x2", maxBetGrp1x2 },
			{ "maxbetgrpmixparlay", maxBetGrpMixParlay },
			{ "maxbetgrpothersodds", maxBetGrpOthersOdds },
			{ "maxbetgrpspecial", maxBetGrpSpecial }
		};

		var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

		var response = new
		{
			Status = "success",
			Data = null as object,
			Message = string.Empty
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/api/sb/register")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(statusCode: HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeGameApiUtensil,
			fakeCryptographyUtensil,
			fakeHttpClient);

		await client.UpdateAccountAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentId: agentId,
			playerCode: playerCode,
			maxBetGrpHdp: maxBetGrpHdp,
			maxBetGrpBasketBall: maxBetGrpBasketBall,
			maxBetGrpOthersSport: maxBetGrpOthersSport,
			maxBetGrp1x2: maxBetGrp1x2,
			maxBetGrpMixParlay: maxBetGrpMixParlay,
			maxBetGrpOthersOdds: maxBetGrpOthersOdds,
			maxBetGrpSpecial: maxBetGrpSpecial);
	}

	[Fact]
	public async Task Maxbetgrpmixparlay_Is_0()
	{
		var apiUrl = new Uri("https://sports-api.ob33club.com/");
		var apiKey = "G4sL7r0kKFI5Mm317w99";
		var agentId = "YYTES12";
		var playerCode = "4ucmqplxb029";
		var maxBetGrpHdp = 269m;
		var maxBetGrpBasketBall = 169m;
		var maxBetGrpOthersSport = 69m;
		var maxBetGrp1x2 = 299m;
		var maxBetGrpMixParlay = 0m;
		var maxBetGrpOthersOdds = 109m;
		var maxBetGrpSpecial = 59m;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
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

		var parameters = new JsonObject
		{
			{ "hash", hash },
			{ "agentId", agentId },
			{ "userId", userId },
			{ "maxbetgrphdp", maxBetGrpHdp },
			{ "maxbetgrpbasketball", maxBetGrpBasketBall },
			{ "maxbetgrpotherssport", maxBetGrpOthersSport },
			{ "maxbetgrp1x2", maxBetGrp1x2 },
			{ "maxbetgrpothersodds", maxBetGrpOthersOdds },
			{ "maxbetgrpspecial", maxBetGrpSpecial }
		};

		var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

		var response = new
		{
			Status = "success",
			Data = null as object,
			Message = string.Empty
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/api/sb/register")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(statusCode: HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeGameApiUtensil,
			fakeCryptographyUtensil,
			fakeHttpClient);

		await client.UpdateAccountAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentId: agentId,
			playerCode: playerCode,
			maxBetGrpHdp: maxBetGrpHdp,
			maxBetGrpBasketBall: maxBetGrpBasketBall,
			maxBetGrpOthersSport: maxBetGrpOthersSport,
			maxBetGrp1x2: maxBetGrp1x2,
			maxBetGrpMixParlay: maxBetGrpMixParlay,
			maxBetGrpOthersOdds: maxBetGrpOthersOdds,
			maxBetGrpSpecial: maxBetGrpSpecial);
	}

	[Fact]
	public async Task MaxBetGrpOthersOdds_Is_0()
	{
		var apiUrl = new Uri("https://sports-api.ob33club.com/");
		var apiKey = "G4sL7r0kKFI5Mm317w99";
		var agentId = "YYTES12";
		var playerCode = "4ucmqplxb029";
		var maxBetGrpHdp = 269m;
		var maxBetGrpBasketBall = 169m;
		var maxBetGrpOthersSport = 69m;
		var maxBetGrp1x2 = 299m;
		var maxBetGrpMixParlay = 199m;
		var maxBetGrpOthersOdds = 0m;
		var maxBetGrpSpecial = 59m;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
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

		var parameters = new JsonObject
		{
			{ "hash", hash },
			{ "agentId", agentId },
			{ "userId", userId },
			{ "maxbetgrphdp", maxBetGrpHdp },
			{ "maxbetgrpbasketball", maxBetGrpBasketBall },
			{ "maxbetgrpotherssport", maxBetGrpOthersSport },
			{ "maxbetgrp1x2", maxBetGrp1x2 },
			{ "maxbetgrpmixparlay", maxBetGrpMixParlay },
			{ "maxbetgrpspecial", maxBetGrpSpecial }
		};

		var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

		var response = new
		{
			Status = "success",
			Data = null as object,
			Message = string.Empty
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/api/sb/register")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(statusCode: HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeGameApiUtensil,
			fakeCryptographyUtensil,
			fakeHttpClient);

		await client.UpdateAccountAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentId: agentId,
			playerCode: playerCode,
			maxBetGrpHdp: maxBetGrpHdp,
			maxBetGrpBasketBall: maxBetGrpBasketBall,
			maxBetGrpOthersSport: maxBetGrpOthersSport,
			maxBetGrp1x2: maxBetGrp1x2,
			maxBetGrpMixParlay: maxBetGrpMixParlay,
			maxBetGrpOthersOdds: maxBetGrpOthersOdds,
			maxBetGrpSpecial: maxBetGrpSpecial);
	}

	[Fact]
	public async Task MaxBetGrpOthersSport_Is_0()
	{
		var apiUrl = new Uri("https://sports-api.ob33club.com/");
		var apiKey = "G4sL7r0kKFI5Mm317w99";
		var agentId = "YYTES12";
		var playerCode = "4ucmqplxb029";
		var maxBetGrpHdp = 269m;
		var maxBetGrpBasketBall = 169m;
		var maxBetGrpOthersSport = 0m;
		var maxBetGrp1x2 = 299m;
		var maxBetGrpMixParlay = 199m;
		var maxBetGrpOthersOdds = 109m;
		var maxBetGrpSpecial = 59m;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
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

		var parameters = new JsonObject
		{
			{ "hash", hash },
			{ "agentId", agentId },
			{ "userId", userId },
			{ "maxbetgrphdp", maxBetGrpHdp },
			{ "maxbetgrpbasketball", maxBetGrpBasketBall },
			{ "maxbetgrp1x2", maxBetGrp1x2 },
			{ "maxbetgrpmixparlay", maxBetGrpMixParlay },
			{ "maxbetgrpothersodds", maxBetGrpOthersOdds },
			{ "maxbetgrpspecial", maxBetGrpSpecial }
		};

		var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

		var response = new
		{
			Status = "success",
			Data = null as object,
			Message = string.Empty
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/api/sb/register")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(statusCode: HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeGameApiUtensil,
			fakeCryptographyUtensil,
			fakeHttpClient);

		await client.UpdateAccountAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentId: agentId,
			playerCode: playerCode,
			maxBetGrpHdp: maxBetGrpHdp,
			maxBetGrpBasketBall: maxBetGrpBasketBall,
			maxBetGrpOthersSport: maxBetGrpOthersSport,
			maxBetGrp1x2: maxBetGrp1x2,
			maxBetGrpMixParlay: maxBetGrpMixParlay,
			maxBetGrpOthersOdds: maxBetGrpOthersOdds,
			maxBetGrpSpecial: maxBetGrpSpecial);
	}

	[Fact]
	public async Task MaxBetGrpSpecial_Is_0()
	{
		var apiUrl = new Uri("https://sports-api.ob33club.com/");
		var apiKey = "G4sL7r0kKFI5Mm317w99";
		var agentId = "YYTES12";
		var playerCode = "4ucmqplxb029";
		var maxBetGrpHdp = 269m;
		var maxBetGrpBasketBall = 169m;
		var maxBetGrpOthersSport = 69m;
		var maxBetGrp1x2 = 299m;
		var maxBetGrpMixParlay = 199m;
		var maxBetGrpOthersOdds = 109m;
		var maxBetGrpSpecial = 0m;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
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

		var parameters = new JsonObject
		{
			{ "hash", hash },
			{ "agentId", agentId },
			{ "userId", userId },
			{ "maxbetgrphdp", maxBetGrpHdp },
			{ "maxbetgrpbasketball", maxBetGrpBasketBall },
			{ "maxbetgrpotherssport", maxBetGrpOthersSport },
			{ "maxbetgrp1x2", maxBetGrp1x2 },
			{ "maxbetgrpmixparlay", maxBetGrpMixParlay },
			{ "maxbetgrpothersodds", maxBetGrpOthersOdds }
		};

		var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

		var response = new
		{
			Status = "success",
			Data = null as object,
			Message = string.Empty
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/api/sb/register")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(statusCode: HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeGameApiUtensil,
			fakeCryptographyUtensil,
			fakeHttpClient);

		await client.UpdateAccountAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentId: agentId,
			playerCode: playerCode,
			maxBetGrpHdp: maxBetGrpHdp,
			maxBetGrpBasketBall: maxBetGrpBasketBall,
			maxBetGrpOthersSport: maxBetGrpOthersSport,
			maxBetGrp1x2: maxBetGrp1x2,
			maxBetGrpMixParlay: maxBetGrpMixParlay,
			maxBetGrpOthersOdds: maxBetGrpOthersOdds,
			maxBetGrpSpecial: maxBetGrpSpecial);
	}

	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://sports-api.ob33club.com/");
		var apiKey = "G4sL7r0kKFI5Mm317w99";
		var agentId = "YYTES12";
		var playerCode = "4ucmqplxb029";
		var maxBetGrpHdp = 269m;
		var maxBetGrpBasketBall = 169m;
		var maxBetGrpOthersSport = 69m;
		var maxBetGrp1x2 = 299m;
		var maxBetGrpMixParlay = 199m;
		var maxBetGrpOthersOdds = 109m;
		var maxBetGrpSpecial = 59m;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
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

		var parameters = new JsonObject
		{
			{ "hash", hash },
			{ "agentId", agentId },
			{ "userId", userId },
			{ "maxbetgrphdp", maxBetGrpHdp },
			{ "maxbetgrpbasketball", maxBetGrpBasketBall },
			{ "maxbetgrpotherssport", maxBetGrpOthersSport },
			{ "maxbetgrp1x2", maxBetGrp1x2 },
			{ "maxbetgrpmixparlay", maxBetGrpMixParlay },
			{ "maxbetgrpothersodds", maxBetGrpOthersOdds },
			{ "maxbetgrpspecial", maxBetGrpSpecial }
		};

		var requestcontent = new StringContent(JsonSerializer.Serialize(parameters), Encoding.UTF8, MediaTypeNames.Application.Json);

		var response = new
		{
			Status = "success",
			Data = null as object,
			Message = string.Empty
		};
		var responseContent = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, MediaTypeNames.Application.Json);

		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, "/api/sb/register")
			.WithContent(await requestcontent.ReadAsStringAsync())
			.Respond(statusCode: HttpStatusCode.OK, responseContent);

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeGameApiUtensil,
			fakeCryptographyUtensil,
			fakeHttpClient);

		await client.UpdateAccountAsync(
			apiUrl: apiUrl,
			apiKey: apiKey,
			agentId: agentId,
			playerCode: playerCode,
			maxBetGrpHdp: maxBetGrpHdp,
			maxBetGrpBasketBall: maxBetGrpBasketBall,
			maxBetGrpOthersSport: maxBetGrpOthersSport,
			maxBetGrp1x2: maxBetGrp1x2,
			maxBetGrpMixParlay: maxBetGrpMixParlay,
			maxBetGrpOthersOdds: maxBetGrpOthersOdds,
			maxBetGrpSpecial: maxBetGrpSpecial);
	}
}
